using AuthenticationService.Application.Commands.Abstractions;
using AuthenticationService.Contracts.Incoming;
using AuthenticationService.Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Threading;
using System.Threading.Tasks;

namespace AuthenticationService.Application.Commands
{
    public class ChangeUserPasswordCommand : BaseCommand<ChangeUserPasswordDto, IdentityResult>
    {
        public ChangeUserPasswordCommand(ChangeUserPasswordDto changePassword) : base(changePassword) { }
    }

    public class ChangeUserPasswordCommandHandler : IRequestHandler<ChangeUserPasswordCommand, IdentityResult>
    {
        private readonly UserManager<User> _userManager;

        public ChangeUserPasswordCommandHandler(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IdentityResult> Handle(ChangeUserPasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByNameAsync(request.Entity.UserName);
            var resultChanging = await _userManager.ChangePasswordAsync(
                user, request.Entity.OldPassword, request.Entity.NewPassword);

            return resultChanging;
        }
    }
}
