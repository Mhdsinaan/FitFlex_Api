using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitFlex.Domain.Entities.Subscription_model
{
    public class CreateUserSubscriptionDto
    {
        public int UserId { get; set; }
        public int SubscriptionId { get; set; }
    }
}
