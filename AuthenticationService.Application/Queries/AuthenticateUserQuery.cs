using AuthenticationService.Application.Commands.Abstractions;
using AuthenticationService.Contracts.Incoming;
using AuthenticationService.Contracts.Outgoing;
using AuthenticationService.Domain.Models;
using AuthenticationService.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Threading;
using System.Threading.Tasks;

namespace AuthenticationService.Application.Queries
{
    public class AuthenticateUserQuery : BaseCommand<AuthenticationUserDto, AuthenticationResponseDto>
    {
        public AuthenticateUserQuery(AuthenticationUserDto userAuthentication) : base(userAuthentication) { }
    }

    public class AuthenticateUserQueryHandler : IRequestHandler<AuthenticateUserQuery, AuthenticationResponseDto>
    {
        private readonly IAuthenticationManager _authenticationManager;
        private readonly UserManager<User> _userManager;

        public AuthenticateUserQueryHandler(
            IAuthenticationManager authenticationManager,
            UserManager<User> userManager)
        {
            _authenticationManager = authenticationManager;
            _userManager = userManager;
        }

        public async Task<AuthenticationResponseDto> Handle(AuthenticateUserQuery request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByNameAsync(request.Entity.UserName);
            if (user == null)
            {
                return null;
            }

            var roles = await _userManager.GetRolesAsync(user);
            var token = await _authenticationManager.CreateToken();

            var authenticationResponse = new AuthenticationResponseDto()
            {
                UserId = user.Id,
                UserName = user.UserName,
                AccessToken = token,
                Roles = roles
            };

            return authenticationResponse;
        }
    }
}
