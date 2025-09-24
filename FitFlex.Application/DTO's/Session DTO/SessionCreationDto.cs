using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FitFlex.Domain.Enum;

namespace FitFlex.Application.DTO_s.Session_DTO
{
    public class SessionCreationDto
    {
        public string Name { get; set; }  
        public SessionTime StartTime { get; set; }
       
        public int TrainerId { get; set; }
    }
}
