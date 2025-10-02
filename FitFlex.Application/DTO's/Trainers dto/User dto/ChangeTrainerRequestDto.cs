using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitFlex.Application.DTO_s.Trainers_dto.User_dto
{
    public class ChangeTrainerRequestDto
    {
        public int UserId { get; set; }
        public int NewTrainerId { get; set; }
    }
}
