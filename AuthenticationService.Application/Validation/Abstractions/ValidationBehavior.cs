using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ValidationException = FluentValidation.ValidationException;

namespace AuthenticationService.Application.Validation.Abstractions
{
    public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;
        private readonly RoleManager<IdentityRole> _roleManager;

        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators, 
            RoleManager<IdentityRole> roleManager)
        {
            _validators = validators;
            _roleManager = roleManager;
        }

        public Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            if (!IsValidConnectionToDB())
            {
                throw new Exception("Invalid connection to DB");
            }

            var context = new ValidationContext<TRequest>(request);
            var failures = _validators
                .Select(v => v.Validate(context))
                .SelectMany(result => result.Errors)
                .Where(f => f != null)
                .ToList();

            if (failures.Count != 0)
            {
                var resultFailures = string.Join("\n\r", failures.Select(f => f.ErrorMessage));
                throw new ValidationException(resultFailures);
            }

            return next();
        }

        private bool IsValidConnectionToDB()
        {
            try
            {
                var roles = _roleManager.Roles.ToList();
                return roles != null;
            }
            catch
            {
                return false;
            }
        }
    }
}
