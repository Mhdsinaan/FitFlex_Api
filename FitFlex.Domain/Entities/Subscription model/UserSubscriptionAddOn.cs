using System;
using System.ComponentModel.DataAnnotations.Schema;
using FitFlex.Application.DTO_s;
using FitFlex.Domain.Enum;

namespace FitFlex.Domain.Entities.Subscription_model
{
    public class UserSubscriptionAddOn : BaseEntity
    {
        public int Id { get; set; }

        public int UserId { get; set; } 

      

        public int UserSubscriptionId { get; set; } 

        public DateTime StartDate { get; set; } = DateTime.UtcNow;

        [ForeignKey("AdditionalPlan")]
        public int AdditionalPlanId { get; set; }

        public DateTime EndDate { get; set; }

        public PaymentStatus PaymentStatus { get; set; } = PaymentStatus.Pending;

        public subscriptionStatus Status { get; set; } = subscriptionStatus.pending;

        public long Price { get; set; }

      
        public virtual UserSubscription UserSubscription { get; set; }
        public virtual AdditionalPlan AdditionalPlan { get; set; }
    }
}
