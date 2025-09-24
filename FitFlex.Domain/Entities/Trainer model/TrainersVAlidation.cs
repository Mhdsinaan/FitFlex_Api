using FluentValidation;
using FitFlex.Domain.Entities.Trainer_model;

namespace FitFlex.Application.DTOs.Dto_validation
{
    public class TrainersValidation : AbstractValidator<Trainer>
    {
        public TrainersValidation()
        {

            RuleFor(t => t.FullName)
                .NotEmpty().WithMessage("Full Name is required")
                .MaximumLength(100).WithMessage("Full Name cannot exceed 100 characters");



            RuleFor(t => t.Email)
                .NotEmpty().WithMessage("Email is required")
                .Must(email => !string.IsNullOrWhiteSpace(email))
                    .WithMessage("Email cannot contain only spaces")
                .Must(email => email.Trim().Contains("@"))
                    .WithMessage("Email must contain '@'")
                .Must(email => email.Trim().Contains("."))
                    .WithMessage("Email must contain '.'")
                .EmailAddress().WithMessage("Invalid Email format");




            RuleFor(t => t.PhoneNumber)
                .NotEmpty().WithMessage("Phone number is required")
                .Matches(@"^[0-9]{10,15}$")
                    .WithMessage("Phone number must be 10–15 digits");
                

            RuleFor(t => t.Gender)
                .NotEmpty().WithMessage("Gender is required")
                .Must(g => g == "Male" || g == "Female" || g == "Other")
                .WithMessage("Gender must be Male, Female, or Other");

          

            
            RuleFor(t => t.ExperienceYears)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Experience cannot be negative");

           

           
            


            //RuleFor(t => t.Status)
            //    .NotEmpty().WithMessage("Status is required")
            //    .Must(s => s == "Active" || s == "Inactive")
            //    .WithMessage("Status must be Active or Inactive");
                
        }
    }
}
