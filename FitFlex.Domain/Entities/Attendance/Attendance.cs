using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FitFlex.Application.DTO_s;
using FitFlex.Domain.Enum;

namespace FitFlex.Domain.Entities.Attendance
{
    public class Attendance:BaseEntity
    {
      
            public int Id { get; set; }
            public int UserId { get; set; }
            public int TrainerId { get; set; }

            public SessionTime Slot { get; set; }
            public AttendanceStatus Status { get; set; }

        
            public DateTime? PunchIn { get; set; }

           
            public DateTime? PunchOut { get; set; }
        }


    }

