using FitFlex.Application.DTO_s.payment_dtos;
using FitFlex.Application.Interfaces;
using FitFlex.Domain.Entities.stripePayment;
using FitFlex.Domain.Entities.Subscription_model;
using FitFlex.Domain.Enum;
using FitFlex.Infrastructure.Interfaces;
using Stripe;

public class PaymentService : IPaymentService
{
    private readonly IRepository<Payment> _paymentRepo;
    private readonly IRepository<SubscriptionPlan> _subscriptionPlanRepo;
    private readonly IRepository<UserSubscription> _userSubscriptionRepo;

    public PaymentService(IRepository<Payment> paymentRepo, IRepository<SubscriptionPlan> subscriptionPlanRepo, IRepository<UserSubscription> userSubscriptionRepo)
    {
        _paymentRepo = paymentRepo;
        _subscriptionPlanRepo = subscriptionPlanRepo;
        _userSubscriptionRepo = userSubscriptionRepo;
    }

    public async Task<PaymentResponseDto> CreateStripePaymentIntentAsync(UserSubscription subscription)
    {
        var existingPayment = (await _paymentRepo.GetAllAsync())
                          .FirstOrDefault(p => p.UserSubscriptionId == subscription.Id
                                            && (p.Status == PaymentStatus.Pending
                                             || p.Status == PaymentStatus.Processing));

        if (existingPayment != null)
        {
            throw new Exception("A payment is already in progress for this subscription. Please complete or cancel it before creating a new one.");
        }

        var subscriptionPlan = await _subscriptionPlanRepo.GetByIdAsync(subscription.SubscriptionId);
        if (subscriptionPlan == null)
            throw new Exception("Subscription plan not found");

       
        var options = new PaymentIntentCreateOptions
        {
            Amount = (long)(subscriptionPlan.Price * 100), 
            Currency = "inr",
            PaymentMethodTypes = new List<string> { "card" }
        };

        var service = new PaymentIntentService();
        var paymentIntent = await service.CreateAsync(options);

        var payment = new Payment
        {
            UserSubscriptionId = subscription.Id,
            StripePaymentIntentId = paymentIntent.Id,
            Amount = subscriptionPlan.Price,   
            Status = PaymentStatus.Pending,   
            Currency = options.Currency,
            ClientSecret = paymentIntent.ClientSecret,
            

            
        };

        await _paymentRepo.AddAsync(payment);
        await _paymentRepo.SaveChangesAsync();

        return new PaymentResponseDto
        {
            Id = payment.Id,
            UserSubscriptionId = payment.UserSubscriptionId,
            StripePaymentIntentId = payment.StripePaymentIntentId,
            Amount = payment.Amount,
            Currency = payment.Currency,
            ClientSecret = payment.ClientSecret,
            Status = payment.Status.ToString(),
            CreatedOn = payment.CreatedOn,
            PaidOn = payment.PaidOn,
            
        };
    }


    public async Task<bool> ConfirmPaymentAsync(string paymentIntentId)
    {
        var payment = (await _paymentRepo.GetAllAsync())
                      .FirstOrDefault(p => p.StripePaymentIntentId == paymentIntentId);

        if (payment == null || payment.Status == PaymentStatus.Paid)
            return false;

        var service = new PaymentIntentService();
        var options = new PaymentIntentConfirmOptions
        {
            PaymentMethod = "pm_card_visa" // test card for dev
        };

        var intent = await service.ConfirmAsync(paymentIntentId, options);

        if (intent.Status == "succeeded")
        {
            payment.Status = PaymentStatus.Paid;
            payment.PaidOn = DateTime.UtcNow;
            await _paymentRepo.SaveChangesAsync();

            var subscription = await _userSubscriptionRepo.GetByIdAsync(payment.UserSubscriptionId);
            if (subscription != null)
            {
                subscription.PaymentStatus = PaymentStatus.Paid;
                subscription.SubscriptionStatus = subscriptionStatus.Active;
                await _userSubscriptionRepo.SaveChangesAsync();
            }
            return true;
        }
        else if (intent.Status == "requires_payment_method" || intent.Status == "requires_action")
        {
            payment.Status = PaymentStatus.Processing; // still ongoing
        }
        else
        {
            payment.Status = PaymentStatus.Failed;
        }

        await _paymentRepo.SaveChangesAsync();
        return false;
    }
}
