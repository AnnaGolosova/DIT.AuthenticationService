using AuthenticationService.Application.Commands;
using AuthenticationService.Contracts.Incoming;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;

namespace AuthenticationService.Controllers
{
    public class AccountController : MediatingControllerBase
    {
        public AccountController(IMediator _mediator) : base(_mediator)
        { }

        [HttpPost]
        public async Task<IActionResult> AuthenticateUser([FromBody] AuthenticationUserDto user, 
            CancellationToken cancellationToken = default) =>
            await ExecuteCommandAsync(new AuthenticateUserCommand(user), cancellationToken: cancellationToken);

    }
}
