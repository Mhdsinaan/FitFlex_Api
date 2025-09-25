using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FitFlex.Domain.Enum;

namespace FitFlex.Application.DTO_s.workout_DTO
{
    public class PunchAttendanceDto
    {
        public int AttendanceId { get; set; }      
        public int TrainerId { get; set; }       
        public SessionTime Slot { get; set; }
    }
}
