using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitFlex.Application.DTO_s.Diet_plan_dtos
{
    public class DietPlanResponseDto
    {
        public int Id { get; set; }              // Diet plan Id
        public int UserId { get; set; }          // User to whom plan is assigned
        public int TrainerId { get; set; }       // Assigned by trainer
        public string PlanName { get; set; }     // Name of plan
        public string Description { get; set; }  // Optional description
        public int Calories { get; set; }        // Total calories for the plan
        public DateTime StartDate { get; set; }  // Start date of plan
        public DateTime EndDate { get; set; }    // End date of plan

        public List<MealResponseDto> Meals { get; set; } = new List<MealResponseDto>(); // All meals
    }
}
