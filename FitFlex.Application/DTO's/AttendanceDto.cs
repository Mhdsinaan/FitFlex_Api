using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FitFlex.Domain.Enum;

namespace FitFlex.Application.DTO_s
{
    public class AttendanceDto
    {
        public int TrainerId { get; set; }
        public SessionTime SlotTime { get; set; }
        public AttendanceStatus Status { get; set; }           
        public DateTime? PunchIn { get; set; }      
        public DateTime? PunchOut { get; set; }     
    }
}
