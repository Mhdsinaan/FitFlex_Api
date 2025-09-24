using System;
using FitFlex.Application.DTO_s;
using FitFlex.Domain.Enum;

namespace FitFlex.Domain.Entities.WorkoutPlan_Model
{
    public class UserWorkoutAssignment:BaseEntity
    {
        public int Id { get; set; }                     
        public int UserId { get; set; }             
        public int WorkoutPlanId { get; set; }          
    
        public AssignmentStatus Status { get; set; }             
       
        public WorkoutPlan WorkoutPlan { get; set; }
    }
}
