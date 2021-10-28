using AuthenticationService.Application.Commands;
using AuthenticationService.Application.Queries;
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

        /// <summary> Create a new user account </summary>
        /// <params name="registerUser"></param>
        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser([FromBody] RegistrationUserDto registerUser,
            CancellationToken cancellationToken) =>
            await ExecuteCommandAsync(new RegisterUserCommand(registerUser), cancellationToken: cancellationToken);

        /// <summary> Authenticate and autorization user if his exists in the database </summary>
        /// <param name="authenticateUser"></param>
        /// <returns> Bearer token with user roles </returns>
        [HttpPost("authenticate")]
        public async Task<IActionResult> AuthenticateUser([FromBody] AuthenticationUserDto authenticateUser,
            CancellationToken cancellationToken) =>
            await ExecuteQueryAsync(new AuthenticateUserQuery(authenticateUser), cancellationToken: cancellationToken);

        /// <summary> Change account password</summary>
        /// <param name="changePassword"></param>
        /// <returns> No content </returns>
        [HttpPut("change-password")]
        public async Task<IActionResult> ChangeUserPassword([FromBody] ChangeUserPasswordDto changePassword,
            CancellationToken cancellationToken) =>
            await ExecuteCommandAsync(new ChangeUserPasswordCommand(changePassword), cancellationToken: cancellationToken);

        /// <summary> Delete user account</summary>
        /// <param name="authenticateUser"></param>
        /// <returns> No content </returns>
        [HttpDelete]
        public async Task<IActionResult> DeleteUser([FromBody] AuthenticationUserDto authenticateUser,
            CancellationToken cancellationToken) =>
            await ExecuteCommandAsync(new DeleteUserCommand(authenticateUser), cancellationToken: cancellationToken);

    }
}