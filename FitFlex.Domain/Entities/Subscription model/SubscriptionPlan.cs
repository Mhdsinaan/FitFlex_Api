using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FitFlex.Application.DTO_s;

namespace FitFlex.Domain.Entities.Subscription_model
{
    public class SubscriptionPlan : BaseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; } 
        public string Description { get; set; }
        public bool IsAdditional { get; set; } = false;
        public long Price { get; set; }
        public int DurationInMonth { get; set; }


        public ICollection<UserSubscription> UserSubscriptions { get; set; }

    }
}

