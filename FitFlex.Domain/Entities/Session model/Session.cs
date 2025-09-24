using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FitFlex.Application.DTO_s;
using FitFlex.Domain.Entities.Trainer_model;
using FitFlex.Domain.Enum;

namespace FitFlex.Domain.Entities.Session_model
{
    public class Session:BaseEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }



        public int TrainerId { get; set; }

        public SessionTime StartTime { get; set; }

        public int MaxParticipants { get; set; }

     
        public int CurrentParticipants { get; set; }

        public bool IsBooked => CurrentParticipants >= MaxParticipants;
    }
}
