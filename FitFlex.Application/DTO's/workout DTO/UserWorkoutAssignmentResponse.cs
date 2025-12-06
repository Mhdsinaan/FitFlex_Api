using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitFlex.Application.DTO_s.workout_DTO
{
    public class UserWorkoutAssignmentResponse
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int WorkoutID{ get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string AssignmentStatus { get; set; }

        public WorkoutPlanResponse WorkoutPlan { get; set; }
    }
}
