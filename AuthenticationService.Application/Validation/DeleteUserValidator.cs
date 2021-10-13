using AuthenticationService.Application.Commands;
using AuthenticationService.Application.Validation.Abstractions.Interfaces;
using FluentValidation;

namespace AuthenticationService.Application.Validation
{
    public class DeleteUserValidator : AbstractValidator<DeleteUserCommand>
    {
        private readonly IValidationConditions _validateConditions;

        public DeleteUserValidator(IValidationConditions validateConditions)
        {
            _validateConditions = validateConditions;
            CreateRules();
        }

        private void CreateRules()
        {
            RuleFor(cmd => cmd.Entity)
                .NotNull()
                .WithMessage(cmd => "Entity is invalid");

            RuleFor(cmd => cmd.Entity.Username)
                .Must(_validateConditions.IsNotNullOrWhitespace)
                .WithMessage(cmd => "Username is required field");

            RuleFor(cmd => cmd.Entity.Password)
                .Must(_validateConditions.IsValidPassword)
                .WithMessage(cmd => "Password must contain upper letter and digit");

            RuleFor(cmd => cmd.Entity)
                .Must(_validateConditions.IsValidAuthenticate)
                .WithMessage(cmd => "Wrong username or password");
        }
    }
}
