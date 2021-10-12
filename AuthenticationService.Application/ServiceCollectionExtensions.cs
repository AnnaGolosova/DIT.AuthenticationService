using AuthenticationService.Application.Validation.Abstractions;
using AuthenticationService.Application.Validation.Abstractions.Interfaces;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace AuthenticationService.Application
{
    public static class ServiceCollectionExtensions
    {
        public static void AddAuthenticationServiceApplication(this IServiceCollection services)
        {
            services.AddScoped<IValidationConditions, ValidationConditions>();
            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddValidators();
        }

        private static void AddValidators(this IServiceCollection services)
        {
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            services.Scan(x =>
            {
                var entryAssembly = Assembly.GetEntryAssembly();
                IEnumerable<Assembly> referencedAssemblies = entryAssembly.GetReferencedAssemblies().Select(Assembly.Load);
                IEnumerable<Assembly> assemblies = new List<Assembly> { entryAssembly }.Concat(referencedAssemblies);

                x.FromAssemblies(assemblies)
                    .AddClasses(classes => classes.AssignableTo(typeof(IValidator<>)))
                    .AsImplementedInterfaces()
                    .WithScopedLifetime();
            });
        }
    }
}
