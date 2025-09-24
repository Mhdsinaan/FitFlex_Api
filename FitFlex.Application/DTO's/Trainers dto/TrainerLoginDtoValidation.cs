using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace FitFlex.Application.DTO_s.Trainers_dto
{
    public class TrainerLoginDtoValidation: AbstractValidator<TrainerLoginDto>
    {
        public TrainerLoginDtoValidation()
        {
            RuleFor(t => t.Email)
                .NotEmpty().WithMessage("Email is required")
                .Must(email => !string.IsNullOrWhiteSpace(email))
                    .WithMessage("Email cannot contain only spaces")
                .Must(email => email.Trim().Contains("@"))
                    .WithMessage("Email must contain '@'")
                .Must(email => email.Trim().Contains("."))
                    .WithMessage("Email must contain '.'")
                .EmailAddress().WithMessage("Invalid Email format");


            RuleFor(t => t.Password)
                .NotEmpty().WithMessage("Password is required")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters long")
                .MaximumLength(50).WithMessage("Password cannot exceed 50 characters");
               
        }
    }
}
