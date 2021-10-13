using AuthenticationService.Application.Commands;
using AuthenticationService.Application.Validation.Abstractions.Interfaces;
using FluentValidation;

namespace AuthenticationService.Application.Validation
{
    public class RegistrationValidator : AbstractValidator<RegisterUserCommand>
    {
        private readonly IValidationConditions _validateConditions;

        public RegistrationValidator(IValidationConditions validateConditions)
        {
            _validateConditions = validateConditions;
            CreateRules();
        }

        private void CreateRules()
        {
            RuleFor(cmd => cmd.Entity)
                .NotNull()
                .WithMessage(cmd => "Entity is invalid");

            RuleFor(cmd => cmd.Entity.UserName)
                .Must(_validateConditions.IsNotNullOrWhitespace)
                .WithMessage(cmd => "Username is required field");

            RuleFor(cmd => cmd.Entity.Password)
                .Must(_validateConditions.IsValidPassword)
                .WithMessage(cmd => "Password must contain upper letter and digit");

            RuleFor(cmd => cmd.Entity.Email)
                .Must(_validateConditions.IsValidEmail)
                .WithMessage(cmd => "Invalid email address");

            RuleFor(cmd => cmd.Entity.Roles)
                .Must(_validateConditions.RolesExists)
                .WithMessage(cmd => $"Invalid roles. Possible roles: Administrator, Moderator and User");
        }
    }
}
