using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitFlex.Application.DTO_s.Trainers_dto
{
    public class TrainerRegisterDto
    {
        public string FullName { get; set; } 
        
        public string Email { get; set; }
        public string Password { get; set; }


        public string PhoneNumber { get; set; }        
        public string Gender { get; set; }           
       
        public int ExperienceYears { get; set; }         
      
    }
}
