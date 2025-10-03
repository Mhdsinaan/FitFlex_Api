using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FitFlex.Application.DTO_s;
using FitFlex.Domain.Entities.Trainer_model;
using FitFlex.Domain.Entities.Users_Model;

namespace FitFlex.Domain.Entities.NewFolder
{
    public class UserDietPlan : BaseEntity
    {
        public int Id { get; set; }

        public int UserId { get; set; }

    
        public string PlanName { get; set; }
        public string Description { get; set; }
        public int Calories { get; set; }
        public string MealDetails { get; set; } 

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public int TrainerId { get; set; }
        public virtual User User { get; set; }
        public virtual Trainer Trainer { get; set; }
    }

}
