using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FitFlex.Application.DTO_s;
using FitFlex.Domain.Enum;

namespace FitFlex.Domain.Entities.Subscription_model
{
    public class UserSubscriptionAddOn : BaseEntity
    {
        public int Id { get; set; }

      
        public int UserSubscriptionId { get; set; }
        public DateTime EndDate { get; set; }
        public PaymentStatus PaymentStatus { get; set; }


        public string FeatureName { get; set; }
        public long Price { get; set; }

       
        public UserSubscription UserSubscription { get; set; }
    }
}
