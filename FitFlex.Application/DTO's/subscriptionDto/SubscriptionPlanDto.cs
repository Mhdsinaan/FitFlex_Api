using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitFlex.Application.DTO_s.subscriptionDto
{
    public class SubscriptionPlanDto
    {
       
        public string Name { get; set; }
        public string Description { get; set; }
        public long Price { get; set; }
        public int DurationInMonth { get; set; }
    }
    public class CreateSubscriptionPlanDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public long Price { get; set; }
        public int DurationInMonth { get; set; }
    }

    public class UpdateSubscriptionPlanDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public long Price { get; set; }
        public int DurationInMonth  { get; set; }
    }
    public class SubscriptionPlansResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public long Price { get; set; }
        public int DurationInMonth { get; set; }
    }

}
