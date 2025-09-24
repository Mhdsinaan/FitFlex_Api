using System;
using FitFlex.Domain.Entities.Subscription_model;
using FitFlex.Domain.Enum;

namespace FitFlex.Domain.Entities.stripePayment
{
    public class Payment
    {
        public int Id { get; set; }

      
        public int UserSubscriptionId { get; set; }
        public UserSubscription UserSubscription { get; set; }

       
        public string StripePaymentIntentId { get; set; } // Stripe Payment Intent ID
        public string ClientSecret { get; set; } // Optional, sent to frontend for completing payment

        // Amount in smallest currency unit (paise for INR)
        public long Amount { get; set; }

        // Currency code
        public string Currency { get; set; } = "inr";

        // Payment status
        public PaymentStatus Status { get; set; } // Pending, Paid, Failed, Refunded

        // When the payment was completed
        public DateTime? PaidOn { get; set; }

        // Timestamps
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
        public DateTime? ModifiedOn { get; set; }
        public bool IsDeleted { get; set; } = false;
    }
}
