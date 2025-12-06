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

       
        public string StripePaymentIntentId { get; set; } 
        public string ClientSecret { get; set; } 

      
        public long Amount { get; set; }

       
        public string Currency { get; set; } = "inr";

    
        public PaymentStatus Status { get; set; }

      
        public DateTime? PaidOn { get; set; }

       
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
        public DateTime? ModifiedOn { get; set; }
        public bool IsDeleted { get; set; } = false;
    }
}
