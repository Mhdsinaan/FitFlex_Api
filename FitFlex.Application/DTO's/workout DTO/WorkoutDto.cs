using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitFlex.Application.DTO_s.workout_DTO
{
    public class WorkoutDto
    {
        public int AssignmentId { get; set; }  
        public int UserId { get; set; }
        public int TrainerId { get; set; }
        public DateTime StartDate { get; set; }
        //public DateTime EndDate { get; set; }
        public string Status { get; set; }     
    }
}
