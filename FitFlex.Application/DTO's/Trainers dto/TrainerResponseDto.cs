using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FitFlex.Domain.Enum;

namespace FitFlex.Application.DTO_s.Trainers_dto
{
    public class TrainerResponseDto
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Gender { get; set; }
        public DateTime CreatedOn { get; set; }

        // Optional
        public int ExperienceYears { get; set; }
        public string Status { get; set; }  
        public string Shift { get; set; }



    }
}
