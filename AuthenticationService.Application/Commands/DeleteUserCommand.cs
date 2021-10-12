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
    public class DeleteUserCommand : BaseCommand<AuthenticationUserDto, Response>
    {
        public DeleteUserCommand(AuthenticationUserDto userAuthentication) : base(userAuthentication) { }
    }

    class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, Response>
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

        public async Task<Response> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var userForDelete = await _userManager.FindByNameAsync(request.Entity.Username);
            var result = await _userManager.DeleteAsync(userForDelete);
            if (result.Succeeded == false)
            {
                return Response.Error;
            }

            return Response.Successfull;
        }
    }
}
