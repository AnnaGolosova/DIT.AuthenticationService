using AuthenticationService.Application.Commands.Abstractions;
using AuthenticationService.Contracts.Incoming;
using AuthenticationService.Contracts.Outgoing;
using AuthenticationService.Domain.Models;
using AuthenticationService.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Threading;
using System.Threading.Tasks;

namespace AuthenticationService.Application.Commands
{
    public class AuthenticateUserCommand : BaseCommand<AuthenticationUserDto, AuthenticationResponseDto>
    {
        public AuthenticateUserCommand(AuthenticationUserDto userAuthentication) : base(userAuthentication) { }
    }

    class AuthenticateUserCommandHandler : IRequestHandler<AuthenticateUserCommand, AuthenticationResponseDto>
    {
        private readonly IAuthenticationManager _authenticationManager;
        private readonly UserManager<User> _userManager;

        public AuthenticateUserCommandHandler(
            IAuthenticationManager authenticationManager,
            UserManager<User> userManager)
        {
            _authenticationManager = authenticationManager;
            _userManager = userManager;
        }

        public async Task<AuthenticationResponseDto> Handle(AuthenticateUserCommand request, CancellationToken cancellationToken)
        {
            var userForRoles = await _userManager.FindByNameAsync(request.Entity.Username);
            var roles = await _userManager.GetRolesAsync(userForRoles);
            var token = await _authenticationManager.CreateToken();

            var authenticationResponse = new AuthenticationResponseDto()
            {
                Token = token,
                Roles = roles
            };

            return authenticationResponse;
        }
    }
}
