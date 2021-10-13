using AuthenticationService.Application.Commands.Abstractions;
using AuthenticationService.Contracts.Incoming;
using AuthenticationService.Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Threading;
using System.Threading.Tasks;

namespace AuthenticationService.Application.Commands
{
    public class RegisterUserCommand : BaseCommand<RegistrationUserDto, IdentityResult>
    {
        public RegisterUserCommand(RegistrationUserDto changePassword) : base(changePassword) { }
    }

    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, IdentityResult>
    {
        private readonly UserManager<User> _userManager;

        public RegisterUserCommandHandler(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IdentityResult> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var userForRegistration = request.Entity;

            var user = MapRegistrationUserDtoToUser(userForRegistration);

            var resultCreating = await _userManager.CreateAsync(user, userForRegistration.Password);
            if (resultCreating.Succeeded == false)
            {
                return resultCreating;
            }

            var resultAddRoles = await _userManager.AddToRolesAsync(user, userForRegistration.Roles);
            if (resultAddRoles.Succeeded == false)
            {
                return resultAddRoles;
            }

            return IdentityResult.Success;
        }

        private User MapRegistrationUserDtoToUser(RegistrationUserDto userForRegistration)
        {
            return new User()
            {
                Email = userForRegistration.Email,
                UserName = userForRegistration.UserName,
            };
        }
    }
}


