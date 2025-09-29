using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FitFlex.Domain.Entities.Subscription_model;
using FitFlex.Domain.Enum;

namespace FitFlex.Application.DTO_s.subscriptionDto
{
    public class UserSubscriptionResponseDto
    {
        public int UserId { get; set; }
        public string  UserName { get; set; }
        public int PlanId { get; set; }
        public string PlanName { get; set; }
        public int TrainerId { get; set; }
        public string TrainerName { get; set; }
        public subscriptionStatus SubscriptionStatus { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public List<SubscriptionPlansResponseDto> AdditionalPlans { get; set; } = new List<SubscriptionPlansResponseDto>();



       
    }
}
