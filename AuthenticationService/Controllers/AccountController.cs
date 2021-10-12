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

        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser([FromBody] RegistrationUserDto registerUser,
            CancellationToken cancellationToken) =>
            await ExecuteCommandAsync(new RegisterUserCommand(registerUser), cancellationToken: cancellationToken);

        [HttpPost("authenticate")]
        public async Task<IActionResult> AuthenticateUser([FromBody] AuthenticationUserDto authenticateUser,
            CancellationToken cancellationToken) =>
            await ExecuteCommandAsync(new AuthenticateUserCommand(authenticateUser), cancellationToken: cancellationToken);

        [HttpPut("change-password")]
        public async Task<IActionResult> ChangeUserPassword([FromBody] ChangeUserPasswordDto changePassword,
            CancellationToken cancellationToken) =>
            await ExecuteCommandAsync(new ChangeUserPasswordCommand(changePassword), cancellationToken: cancellationToken);
        
        [HttpDelete()]
        public async Task<IActionResult> DeleteUser([FromBody] AuthenticationUserDto authenticateUser,
            CancellationToken cancellationToken) =>
            await ExecuteCommandAsync(new DeleteUserCommand(authenticateUser), cancellationToken: cancellationToken);

    }
}
