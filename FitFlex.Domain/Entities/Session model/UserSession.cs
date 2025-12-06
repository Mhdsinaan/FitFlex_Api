using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FitFlex.Application.DTO_s;
using FitFlex.Domain.Entities.Users_Model;

namespace FitFlex.Domain.Entities.Session_model
{
    public class UserSession:BaseEntity
    {
        public int Id { get; set; }
        public int TrainerID { get; set; }
        public int SessionId { get; set; }

        //public virtual User User { get; set; }
        public virtual Session Session { get; set; }
    }
}
