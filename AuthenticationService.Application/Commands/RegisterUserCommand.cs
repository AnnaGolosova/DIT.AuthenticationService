using AuthenticationService.Application.Commands.Abstractions;
using AuthenticationService.Contracts.Incoming;
using AuthenticationService.Contracts.Outgoing.Abstractions;
using AuthenticationService.Domain.Models;
using AuthenticationService.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Threading;
using System.Threading.Tasks;

namespace AuthenticationService.Application.Commands
{
    public class RegisterUserCommand : BaseCommand<RegistrationUserDto, Response>
    {
        public RegisterUserCommand(RegistrationUserDto changePassword) : base(changePassword) { }
    }

    class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, Response>
    {
        private readonly IAuthenticationManager _authenticationManager;
        private readonly UserManager<User> _userManager;

        public RegisterUserCommandHandler(IAuthenticationManager authenticationManager,
            UserManager<User> userManager)
        {
            _authenticationManager = authenticationManager;
            _userManager = userManager;
        }

        public async Task<Response> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var userForRegistration = request.Entity;

            var user = MapRegistrationUserDtoToUser(userForRegistration);

            var result = await _userManager.CreateAsync(user, userForRegistration.Password);
            if (result.Succeeded == false)
            {
                throw new System.Exception(result.Errors.ToString());
            }

            await _userManager.AddToRolesAsync(user, userForRegistration.Roles);

            return Response.Successfull;
        }

        private User MapRegistrationUserDtoToUser(RegistrationUserDto userForRegistration)
        {
            return new User()
            {
                Email = userForRegistration.Email,
                UserName = userForRegistration.Username,
            };
        }
    }
}


