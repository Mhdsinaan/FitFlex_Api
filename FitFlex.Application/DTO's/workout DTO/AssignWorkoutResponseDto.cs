using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitFlex.Application.DTO_s.workout_DTO
{
    public class AssignWorkoutResponseDto
    {
        public int UserId { get; set; }
        public int WorkoutId { get; set; }
        public int TrainerId { get; set; }
        public string AssignmentStatus { get; set; }
        
    }

}
