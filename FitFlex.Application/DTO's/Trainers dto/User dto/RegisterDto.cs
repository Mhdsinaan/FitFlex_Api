using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FitFlex.Domain.Enum;

namespace FitFlex.Application.DTO_s.User_dto
{
    public class RegisterDto
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        

        public DateTime JoiningDate { get; set; }





    }
}
