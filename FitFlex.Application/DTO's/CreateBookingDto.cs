using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FitFlex.Domain.Enum;

namespace FitFlex.Application.DTO_s
{
    public class CreateBookingDto
    {
      
        public int UserId { get; set; }     
        public int TrainerId { get; set; }
      
        public DateTime BookingDate { get; set; }
        public Trainershift shift { get; set; }
    }
}
