using System;
using System.Collections.Generic;
using FitFlex.Application.DTO_s;
using FitFlex.Domain.Enum;


namespace FitFlex.Domain.Entities.WorkoutPlan_Model
{
    public class WorkoutPlan:BaseEntity
    {
        public int Id { get; set; }                
        public string Name { get; set; }         
        public string Description { get; set; }    
        public WorkoutLevel Level { get; set; }        
       
        public DateTime CreatedDate { get; set; }  
       
        public ICollection<WorkoutExercise> Exercises { get; set; }
        public ICollection<UserWorkoutAssignment> Assignments { get; set; }
    }
}
