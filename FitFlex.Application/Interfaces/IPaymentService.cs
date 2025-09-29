using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FitFlex.Application.DTO_s.payment_dtos;
using FitFlex.CommenAPi;
using FitFlex.Domain.Entities.stripePayment;
using FitFlex.Domain.Entities.Subscription_model;

namespace FitFlex.Application.Interfaces
{
    public interface IPaymentService
    {
        Task<APiResponds<bool>> ConfirmPaymentForMultipleAsync(string paymentIntentId, List<UserSubscriptionAddOn> addOns);



        Task<APiResponds<PaymentResponseDto>> CreateStripePaymentIntentForMultipleAsync(UserSubscription mainSubscription, List<UserSubscriptionAddOn> addOns);


    }
}
