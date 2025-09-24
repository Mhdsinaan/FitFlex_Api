using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FitFlex.Application.services;
using FitFlex.Domain.Enum;

namespace FitFlex.Application.DTO_s.Trainers_dto
{
    public class TrainerUpdateDto
    {
        public string? FullName { get; set; }     
        public string? PhoneNumber { get; set; }  
        public string? Gender { get; set; }     
        public int ExperienceYears { get; set; }
        public TrainerStatus status { get; set; }
    }
}
