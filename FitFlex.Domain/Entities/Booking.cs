using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FitFlex.Application.DTO_s;
using FitFlex.Domain.Entities.Trainer_model;
using FitFlex.Domain.Entities.Users_Model;
using FitFlex.Domain.Enum;

namespace FitFlex.Domain.Entities
{
    public class Booking:BaseEntity
    {
        public int Id { get; set; }
        //public int userplanID { get; set; }
        public int UserID { get; set; }
        public int TrainerId { get; set; }
        public DateTime Date { get; set; }
        public Trainershift Shift { get; set; }

        public virtual User User { get; set; }
        public virtual Trainer Trainer { get; set; }
    }
}
