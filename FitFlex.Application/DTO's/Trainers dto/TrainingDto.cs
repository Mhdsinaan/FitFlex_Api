using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitFlex.Application.DTO_s.Trainers_dto
{
    public class TrainingDto
    {
              
        public int PlanId { get; set; }          
        public int UserId { get; set; }              
        public int TrainerId { get; set; }           
        public string TrainerName { get; set; }       
        public DateTime Date { get; set; }           
        public string Shift { get; set; }            
        public string Status { get; set; }
    }
}
