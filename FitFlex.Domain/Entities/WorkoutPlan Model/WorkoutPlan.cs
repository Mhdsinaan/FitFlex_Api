using System;
using System.Collections.Generic;
using FitFlex.Application.DTO_s;

namespace FitFlex.Domain.Entities.WorkoutPlan_Model
{
    public class WorkoutPlan:BaseEntity
    {
        public int Id { get; set; }                // PK
        public string Name { get; set; }           // Plan name (e.g., Push Pull Legs)
        public string Description { get; set; }    // Plan description
        public string Level { get; set; }          // Beginner/Intermediate/Advanced
        // TrainerId
        public DateTime CreatedDate { get; set; }  // When plan was created
        // 🔗 Relationships
        public ICollection<WorkoutExercise> Exercises { get; set; }
        public ICollection<UserWorkoutAssignment> Assignments { get; set; }
    }
}
