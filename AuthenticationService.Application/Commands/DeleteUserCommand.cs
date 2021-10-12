using AuthenticationService.Application.Commands.Abstractions;
using AuthenticationService.Contracts.Incoming;
using AuthenticationService.Domain.Models;
using AuthenticationService.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Threading;
using System.Threading.Tasks;

namespace AuthenticationService.Application.Commands
{
    public class DeleteUserCommand : BaseCommand<AuthenticationUserDto, IdentityResult>
    {
        public DeleteUserCommand(AuthenticationUserDto userAuthentication) : base(userAuthentication) { }
    }

    class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, IdentityResult>
    {
        private readonly IAuthenticationManager _authenticationManager;
        private readonly UserManager<User> _userManager;

        public DeleteUserCommandHandler(
            IAuthenticationManager authenticationManager,
            UserManager<User> userManager)
        {
            _authenticationManager = authenticationManager;
            _userManager = userManager;
        }

        public async Task<IdentityResult> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var userForDelete = await _userManager.FindByNameAsync(request.Entity.Username);
            var result = await _userManager.DeleteAsync(userForDelete);

            return result;
        }
    }
}
