using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitFlex.Application.DTO_s.Diet_plan_dtos
{
    public class DietPlanUpdateDto
    {
        public string PlanName { get; set; }

      
        public string Description { get; set; }

        public int? Calories { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public List<MealUpdateDto> Meals { get; set; }
    }
}
