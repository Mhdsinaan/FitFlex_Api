using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitFlex.Application.DTO_s.workout_DTO
{
    public class AssignWorkoutRequest
    {
        public string UserId { get; set; }
        public int PlanId { get; set; }
        public string TrainerId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }


}
