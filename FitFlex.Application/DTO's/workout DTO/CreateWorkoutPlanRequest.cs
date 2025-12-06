using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FitFlex.Domain.Entities.WorkoutPlan_Model;
using FitFlex.Domain.Enum;

namespace FitFlex.Application.DTO_s.workout_DTO
{
    public class CreateWorkoutPlanRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public WorkoutLevel Level { get; set; } 
        //public int TrainerId { get; set; }

        public List<CreateWorkoutExerciseDto> Exercises { get; set; } = new();

    }
}
