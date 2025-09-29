using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitFlex.Application.DTO_s.subscriptionDto
{
    public class SubscriptionSelectionDto
    {
        public int PlanId { get; set; } 

        public List<int>? AdditionalPlanIds { get; set; }
    }
}
