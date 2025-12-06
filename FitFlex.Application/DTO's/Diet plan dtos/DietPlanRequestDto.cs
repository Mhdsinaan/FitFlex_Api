using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitFlex.Application.DTO_s.Diet_plan_dtos
{
    public class DietPlanRequestDto
    {
        public int UserId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string PlanName { get; set; }
        public string Description { get; set; }  
        public int Calories { get; set; } 

        public string Breakfast { get; set; }
        public int BreakfastCalories { get; set; }

        public string MidMeal { get; set; }
        public int MidMealCalories { get; set; }

        public string Lunch { get; set; }
        public int LunchCalories { get; set; }

        public string EveningSnack { get; set; }
        public int EveningSnackCalories { get; set; }

        public string Dinner { get; set; }
        public int DinnerCalories { get; set; }
        public List<MealRequestDto> Meals { get; set; }
    }
}
