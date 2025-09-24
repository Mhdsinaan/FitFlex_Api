using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.AspNetCore.Identity;

namespace FitFlex.Domain.Entities.Users_Model
{
    public class UserValidation : AbstractValidator<User>
    {
        public UserValidation()
        {
            RuleFor(x => x.UserName)
                   .NotEmpty().WithMessage("UserName is required")
                   .MaximumLength(50).WithMessage("UserName cannot exceed 50 characters")
                   .Must(name => !string.IsNullOrWhiteSpace(name) && !name.Contains(" "))
                   .WithMessage("UserName cannot contain whitespace");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Invalid email format");

            RuleFor(x => x.Password)
               .NotEmpty().WithMessage("Password is required")
               .MinimumLength(6).WithMessage("Password must be at least 6 characters long");



        }

    }
}
