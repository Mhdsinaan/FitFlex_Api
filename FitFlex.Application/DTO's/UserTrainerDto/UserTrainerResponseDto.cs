using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FitFlex.Domain.Enum;

namespace FitFlex.Application.DTO_s.UserTrainerDto
{
    public class UserTrainerResponseDto
    {
               
        public int UserId { get; set; }      
        public string UserName { get; set; } 
        public int TrainerId { get; set; }  
        public string TrainerName { get; set; } 
        public DateTime AssignedDate { get; set; }
        public string shift { get; set; }
    }
}
