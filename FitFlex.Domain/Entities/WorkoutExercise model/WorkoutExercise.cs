using System;
using FitFlex.Application.DTO_s;

namespace FitFlex.Domain.Entities.WorkoutPlan_Model
{
    public class WorkoutExercise:BaseEntity
    {
        public int Id { get; set; }                  // PK
        public int WorkoutPlanId { get; set; }       // FK
        public string ExerciseName { get; set; }     // e.g., Bench Press
        public int? Sets { get; set; }               // nullable (for cardio)
        public int? Reps { get; set; }               // nullable (for cardio)
          // e.g., "20 min"

        // 🔗 Navigation property
        public WorkoutPlan WorkoutPlan { get; set; }
    }
}
