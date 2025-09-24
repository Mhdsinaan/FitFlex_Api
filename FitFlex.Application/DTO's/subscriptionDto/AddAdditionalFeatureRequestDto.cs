using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitFlex.Application.DTO_s.subscriptionDto
{
    public class AddAdditionalFeatureRequestDto
    {
        //public int UserSubscriptionId { get; set; }
        //public int SubscriptionPlanId { get; set; }
        //public DateTime StartDate { get; set; }
        //public DateTime EndDate { get; set; }
        public int UserSubscriptionId { get; set; }   // Main subscription ID
        public string FeatureName { get; set; }       // Name of the add-on, e.g., "Treadmill"
        public long Price { get; set; }               // Cost of the add-on
        public DateTime StartDate { get; set; }       // When the add-on starts
        public DateTime EndDate { get; set; }         // When the add-on ends
    }
}
