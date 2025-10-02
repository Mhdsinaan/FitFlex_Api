using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitFlex.Application.DTO_s.Session_DTO
{
    public class UserSessionResponseDto
    {
      
        public int UserId { get; set; }
        public string UserName { get; set; }
        public int SessionId { get; set; }
        public string SessionName { get; set; }
        public int TrainerId { get; set; }
        public string TrainerName { get; set; }
        public string Status { get; set; }   
    }
}
