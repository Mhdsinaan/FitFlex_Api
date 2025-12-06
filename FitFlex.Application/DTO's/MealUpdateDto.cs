using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitFlex.Application.DTO_s
{
    public class MealUpdateDto
    {
        public int? MealId { get; set; } 
        public string MealType { get; set; } 
        public string FoodItems { get; set; }
        public int? Calories { get; set; }
    }
}
