using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FitFlex.Domain.Enum;

namespace FitFlex.Domain.Entities.Booking_model
{
    public class BookingResponseDto
    {

        public int UserId { get; set; }
        //public string UserName { get; set; }
        //public string TrainerName { get; set; }


        public int TrainerId { get; set; }
      

        public DateTime BookingDate { get; set; }
        public string Session { get; set; }
        //public string Status { get; set; }
    }
}
