using AuthenticationService.Application.Commands;
using AuthenticationService.Application.Queries;
using AuthenticationService.Contracts.Incoming;
using AuthenticationService.Controllers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Threading;
using Xunit;

namespace AuthenticationService.UnitTests.AuthenticationServiceTests.Controllers
{
    public class AccountControllerTests
    {
        private Mock<IMediator> _mediatr;
        private AccountController _controller;

        private AuthenticationUserDto _authenticateUser;
        private ChangeUserPasswordDto _changePassword;
        private RegistrationUserDto _registerUser;

        private RegisterUserCommand _registerUserCommand;
        private AuthenticateUserQuery _AuthenticateUserQuery;
        private ChangeUserPasswordCommand _changePasswordCommand;
        private DeleteUserCommand _deleteUserCommand;

        public AccountControllerTests()
        {
            _mediatr = new Mock<IMediator>();
            _controller = new AccountController(_mediatr.Object);

            InitEnities();
            InitCommands();
        }

        private void InitEnities()
        {
            _authenticateUser = new AuthenticationUserDto()
            {
                UserName = "Test",
                Password = "Password1",
            };
            _changePassword = new ChangeUserPasswordDto()
            {
                UserName = "Test",
                OldPassword = "OldPassword1",
                NewPassword = "NewPassword1",
            };
            _registerUser = new RegistrationUserDto()
            {
                UserName = "Test",
                Password = "OldPassword1",
                Email = "test@test.ru",
                Roles = new string[] { "User", "Moderator" },
            };
        }

        private void InitCommands()
        {
            _registerUserCommand = new RegisterUserCommand(_registerUser);
            _AuthenticateUserQuery = new AuthenticateUserQuery(_authenticateUser);
            _changePasswordCommand = new ChangeUserPasswordCommand(_changePassword);
            _deleteUserCommand = new DeleteUserCommand(_authenticateUser);
        }

        //[Fact]
        //public async void RegisterUser_ValidRegisterUser_OkObjectResult()
        //{
        //    var registerResult = await _controller.RegisterUser(_registerUser, CancellationToken.None);

        //    Assert.IsType<OkObjectResult>(registerResult);
        //}
    }
}
