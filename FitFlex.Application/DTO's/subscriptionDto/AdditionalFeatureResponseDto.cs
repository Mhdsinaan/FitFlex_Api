using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitFlex.Application.DTO_s.subscriptionDto
{
    public class AdditionalFeatureResponseDto
    {
        public int UserID { get; set; }               
        public string FeatureName { get; set; }

        public decimal Price { get; set; }          
        public DateTime StartDate { get; set; }     
        public DateTime EndDate { get; set; }       
        public string PaymentStatus { get; set; }
        public string  Status { get; set; }
    }
}
