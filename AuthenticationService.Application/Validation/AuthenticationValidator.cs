﻿using AuthenticationService.Application.Commands.Abstractions;
using AuthenticationService.Application.Validation.Abstractions;
using AuthenticationService.Contracts.Incoming;
using FluentValidation;
using System;
using System.Linq;

namespace AuthenticationService.Application.Validation
{
    public class AuthenticationValidator<TCommand, TResponse> : AbstractValidator<TCommand>
        where TCommand : BaseCommand<AuthenticationUserDto, TResponse>
    {
        public AuthenticationValidator()
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

            RuleFor(cmd => cmd.Entity.Password)
                .Must(ValidationConditions.IsNotNullOrWhitespace)
                .WithMessage(cmd => "Entity");

            RuleFor(cmd => cmd.Entity.Password)
                .Must(ValidationConditions.IsValidPassword)
                .WithMessage(cmd => "Entity");
        }

    }
}