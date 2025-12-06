using System;
using System.ComponentModel.DataAnnotations;
using FitFlex.Application.DTO_s;
using FitFlex.Domain.Entities.Trainer_model;
using FitFlex.Domain.Enum;

namespace FitFlex.Domain.Entities.WorkoutPlan_Model
{
    public class UserWorkoutAssignment:BaseEntity
    {
        
        public int Id { get; set; }                     
        public int UserId { get; set; }             
        public int WorkoutPlanId { get; set; }
        public int TrainerId { get; set; }
        public AssignmentStatus AssignmentStatus { get; set; }

        public WorkoutPlan WorkoutPlan { get; set; }

        public Trainer trainer { get; set; }
    }
}
    