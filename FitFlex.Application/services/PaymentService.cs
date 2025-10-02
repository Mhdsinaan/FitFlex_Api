using FitFlex.Application.DTO_s.payment_dtos;
using FitFlex.Application.Interfaces;
using FitFlex.CommenAPi;
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
    private readonly IRepository<AdditionalPlan> _AdditionalSubscriptionRepo;


    public PaymentService(
        IRepository<Payment> paymentRepo,
        IRepository<SubscriptionPlan> subscriptionPlanRepo,
        IRepository<UserSubscription> userSubscriptionRepo,
        IRepository<AdditionalPlan> AdditionalSubscriptionRepo)
    {
        _paymentRepo = paymentRepo;
        _subscriptionPlanRepo = subscriptionPlanRepo;
        _userSubscriptionRepo = userSubscriptionRepo;
        _AdditionalSubscriptionRepo = AdditionalSubscriptionRepo;
    }

    public async Task<APiResponds<PaymentResponseDto>> CreateStripePaymentIntentForMultipleAsync(
     UserSubscription mainSubscription,
     List<UserSubscriptionAddOn> addOns)
    {
        try
        {
            
            if (mainSubscription == null)
                return new APiResponds<PaymentResponseDto>("404", "Main subscription is null", null);

            var existingPayment = (await _paymentRepo.GetAllAsync())
                .FirstOrDefault(p =>
                    (p.UserSubscriptionId == mainSubscription.Id
                    || addOns.Select(a => a.UserSubscriptionId).Contains(p.UserSubscriptionId))
                    && (p.Status == PaymentStatus.Pending || p.Status == PaymentStatus.Processing));

            if (existingPayment != null)
                return new APiResponds<PaymentResponseDto>("400", "A payment is already in progress", null);

            long totalAmount = 0;

            var mainPlan = await _subscriptionPlanRepo.GetByIdAsync(mainSubscription.SubscriptionId);
            if (mainPlan == null)
                return new APiResponds<PaymentResponseDto>("404", "Main subscription plan not found", null);

            totalAmount += mainPlan.Price;

            foreach (var addOn in addOns)
            {
                var addOnPlan = await _AdditionalSubscriptionRepo.GetByIdAsync(addOn.AdditionalPlanId);

                if (addOnPlan != null && addOnPlan.Price > 0 && addOnPlan.IsDelete == false)
                {
                    totalAmount += addOnPlan.Price;
                }
            }


            var options = new PaymentIntentCreateOptions
            {
                Amount = (long)(totalAmount * 100), 
                Currency = "inr",
                PaymentMethodTypes = new List<string> { "card" }
            };

            var service = new PaymentIntentService();
            var paymentIntent = await service.CreateAsync(options);

           
            var payment = new Payment
            {
                UserSubscriptionId = mainSubscription.Id,
                StripePaymentIntentId = paymentIntent.Id,
                Amount = totalAmount, 
                Status = PaymentStatus.Pending,
                Currency = options.Currency,
                ClientSecret = paymentIntent.ClientSecret,
                CreatedOn = DateTime.UtcNow
            };

            await _paymentRepo.AddAsync(payment);
            await _paymentRepo.SaveChangesAsync();

          
            var response = new PaymentResponseDto
            {
                Id = payment.Id,
                UserSubscriptionId = mainSubscription.Id,
                StripePaymentIntentId = payment.StripePaymentIntentId,
                Amount = payment.Amount, 
                Currency = payment.Currency,
                ClientSecret = payment.ClientSecret,
                Status = payment.Status.ToString(),
                CreatedOn = payment.CreatedOn,
                PaidOn = payment.PaidOn
            };

            return new APiResponds<PaymentResponseDto>("200", "Payment intent created successfully", response);
        }
        catch (Exception ex)
        {
            return new APiResponds<PaymentResponseDto>("500", $"Error: {ex.Message}", null);
        }
    }


    public async Task<APiResponds<bool>> ConfirmPaymentForMultipleAsync(string paymentIntentId, List<UserSubscriptionAddOn> addOns)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(paymentIntentId))
                return new APiResponds<bool>("400", "PaymentIntentId is required", false);

            var payment = (await _paymentRepo.GetAllAsync())
                          .FirstOrDefault(p => p.StripePaymentIntentId == paymentIntentId);

            if (payment == null)
                return new APiResponds<bool>("404", "Payment not found", false);

            if (payment.Status == PaymentStatus.Paid)
                return new APiResponds<bool>("400", "Payment already confirmed", false);

            var service = new PaymentIntentService();
            var options = new PaymentIntentConfirmOptions
            {
                PaymentMethod = "pm_card_visa" 
            };

            var intent = await service.ConfirmAsync(paymentIntentId, options);

            if (intent.Status == "succeeded")
            {
                payment.Status = PaymentStatus.Paid;
                payment.PaidOn = DateTime.UtcNow;
                await _paymentRepo.SaveChangesAsync();

                
                var mainSubscription = await _userSubscriptionRepo.GetByIdAsync(payment.UserSubscriptionId);
                if (mainSubscription != null)
                {
                    mainSubscription.PaymentStatus = PaymentStatus.Paid;
                    mainSubscription.SubscriptionStatus = subscriptionStatus.Active;
                }

              
                foreach (var addOn in addOns)
                {
                    addOn.PaymentStatus = PaymentStatus.Paid;
                    addOn.Status = subscriptionStatus.Active;
                }

                await _userSubscriptionRepo.SaveChangesAsync();
                return new APiResponds<bool>("200", "Payment confirmed successfully", true);
            }
            else
            {
                payment.Status = intent.Status == "requires_payment_method" || intent.Status == "requires_action"
                    ? PaymentStatus.Processing
                    : PaymentStatus.Failed;

                await _paymentRepo.SaveChangesAsync();
                return new APiResponds<bool>("400", "Payment confirmation failed", false);
            }
        }
        catch (Exception ex)
        {
            return new APiResponds<bool>("500", $"Error: {ex.Message}", false);
        }
    }

}
