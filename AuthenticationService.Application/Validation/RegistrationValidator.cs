using AuthenticationService.Application.Commands.Abstractions;
using AuthenticationService.Application.Validation.Abstractions;
using AuthenticationService.Contracts.Incoming;
using FluentValidation;

namespace AuthenticationService.Application.Validation
{
    public class RegistrationValidator<TCommand, TResponse> : AbstractValidator<TCommand>
        where TCommand : BaseCommand<RegistrationUserDto, TResponse>
    {
        public RegistrationValidator()
        {
            CreateRules();
        }

        private void CreateRules()
        {
            RuleFor(cmd => cmd.Entity)
                .NotNull()
                .WithMessage(cmd => "Entity is invalid");

            RuleFor(cmd => cmd.Entity.Username)
                .Must(ValidationConditions.IsNotNullOrWhitespace)
                .WithMessage(cmd => "Username is required field");

            RuleFor(cmd => cmd.Entity.Password)
                .Must(ValidationConditions.IsValidPassword)
                .WithMessage(cmd => "Password must contain upper letter and digit");

            RuleFor(cmd => cmd.Entity.Email)
                .Must(ValidationConditions.IsValidEmail)
                .WithMessage(cmd => "Invalid email address");

            RuleFor(cmd => cmd.Entity.Roles)
                .Must(ValidationConditions.IsValidRoles)
                .WithMessage(cmd => $"Invalid roles. Possible roles: Administrator, Moderator and User");
        }
    }
}
