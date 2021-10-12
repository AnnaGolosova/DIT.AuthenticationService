﻿using AuthenticationService.Application.Commands.Abstractions;
using AuthenticationService.Application.Validation.Abstractions;
using AuthenticationService.Contracts.Incoming;
using FluentValidation;

namespace AuthenticationService.Application.Validation
{
    class ChangeUserPasswordValidator<TCommand, TResponse> : AbstractValidator<TCommand>
        where TCommand : BaseCommand<ChangeUserPasswordDto, TResponse>
    {
        public ChangeUserPasswordValidator()
        {
            CreateRules();
        }

        private void CreateRules()
        {
            RuleFor(cmd => cmd.Entity)
                .NotNull()
                .WithMessage(cmd => "Entity");

            RuleFor(cmd => cmd.Entity.Username)
                .Must(ValidationConditions.IsNotNullOrWhitespace)
                .WithMessage(cmd => "Entity");

            RuleFor(cmd => cmd.Entity.OldPassword)
                .Must(ValidationConditions.IsValidPassword)
                .WithMessage(cmd => "Entity");

            RuleFor(cmd => cmd.Entity.NewPassword)
                .Must(ValidationConditions.IsValidPassword)
                .WithMessage(cmd => "Entity");
        }
    }
}
