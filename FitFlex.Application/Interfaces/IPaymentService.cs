using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FitFlex.Application.DTO_s.payment_dtos;
using FitFlex.Domain.Entities.stripePayment;
using FitFlex.Domain.Entities.Subscription_model;

namespace FitFlex.Application.Interfaces
{
    public interface IPaymentService
    {
        Task<PaymentResponseDto> CreateStripePaymentIntentAsync(UserSubscription subscription);
        Task<bool> ConfirmPaymentAsync(string paymentIntentId);
    }
}
