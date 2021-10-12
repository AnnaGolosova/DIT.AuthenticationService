using AuthenticationService.Application.Commands.Abstractions;
using AuthenticationService.Application.Validation.Abstractions.Interfaces;
using AuthenticationService.Contracts.Incoming;
using FluentValidation;

namespace AuthenticationService.Application.Validation
{
    public class DeleteUserValidator<TCommand, TResponse> : AbstractValidator<TCommand>
        where TCommand : BaseCommand<AuthenticationUserDto, TResponse>
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
        }
    }
}
