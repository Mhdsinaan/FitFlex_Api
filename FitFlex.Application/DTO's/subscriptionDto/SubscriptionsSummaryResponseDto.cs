using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitFlex.Application.DTO_s.subscriptionDto
{
    public class SubscriptionsSummaryResponseDto
    {
        public int ActiveSubscriptionsCount { get; set; }   
        public int ExpiredSubscriptionsCount { get; set; }   
        public int NewSubscriptionsCount { get; set; }     
        public decimal TotalRevenue { get; set; }           
       
    }
}
