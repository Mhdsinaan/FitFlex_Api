using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FitFlex.Domain.Enum;

namespace FitFlex.Application.DTO_s.Session_DTO
{
    public class SessionResponseDto
    {
        public int Id { get; set; }
        public string SessionName { get; set; }
        public string Details{ get; set; }
        public string  TimeSlot { get; set; }   
       
    }
}
