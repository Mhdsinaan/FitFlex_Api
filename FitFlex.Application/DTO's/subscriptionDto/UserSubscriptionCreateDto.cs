using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FitFlex.Domain.Enum;

namespace FitFlex.Application.DTO_s.subscriptionDto
{
    public class UserSubscriptionCreateDto
    {
        public int UserID { get; set; }
        public int TrainerId { get; set; }
        public int SubscriptionId { get; set; }
        public subscriptionStatus  Subscriptionstatus { get; set; }
        public PaymentStatus pymentstatus { get; set; }
    }
}
