using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FitFlex.Application.DTO_s.User_dto;
using FluentValidation;

namespace FitFlex.Application.DTO_s.Dto_validation
{
    public class RegisterdtoValidation:AbstractValidator<RegisterDto>
    {
       public RegisterdtoValidation()
        {
            RuleFor(x => x.UserName)
                 .NotEmpty().WithMessage("UserName is required")
                 .MaximumLength(50).WithMessage("UserName cannot exceed 50 characters")
                 .Must(name => !string.IsNullOrWhiteSpace(name) && !name.Contains(" "))
                 .WithMessage("UserName cannot contain whitespace");

            RuleFor(x => x.Email)
                  .NotEmpty().WithMessage("Email is required")
                  .EmailAddress().WithMessage("Invalid email format")
                  .Matches(@"^[^@\s]+@[^@\s]+\.[^@\s]+$").WithMessage("Invalid email format");



            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters long");
                

           
           
        }

    }
}
