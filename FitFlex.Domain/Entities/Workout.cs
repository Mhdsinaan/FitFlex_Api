using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FitFlex.Domain.Entities.Trainer_model;
using FitFlex.Domain.Entities.Users_Model;

namespace FitFlex.Domain.Entities
{
    public class Workout
    {
        public int WorkoutId { get; set; }      

       
        public int UserId { get; set; }         
        public User User { get; set; }

        public int TrainerId { get; set; }      
        public Trainer Trainer { get; set; }

        
        public string Title { get; set; }      
        public string Description { get; set; } 
        public string DayOfWeek { get; set; }   
        public string Duration { get; set; }   

        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
       
    }
}
