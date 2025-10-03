using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FitFlex.Application.DTO_s;
using FitFlex.Domain.Entities.NewFolder;

namespace FitFlex.Domain.Entities.Meal
{
    public class Meal : BaseEntity
    {
        public int Id { get; set; }
        public int DietPlanId { get; set; }  // Foreign key to UserDietPlan
        public string MealType { get; set; } // Breakfast, MidMeal, Lunch, Dinner
        public string FoodItems { get; set; }
        public int Calories { get; set; }

        public virtual UserDietPlan DietPlan { get; set; }
    }
}

