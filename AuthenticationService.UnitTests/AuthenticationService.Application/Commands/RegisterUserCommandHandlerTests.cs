using AuthenticationService.Application.Commands;
using AuthenticationService.Contracts.Incoming;
using AuthenticationService.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Moq;
using System.Threading;
using Xunit;

namespace AuthenticationService.UnitTests.AuthenticationService.Application.Commands
{
    public class RegisterUserCommandHandlerTests
    {
        private RegisterUserCommandHandler _registerUserCommandHandler;
        private RegisterUserCommand _registerUserCommand;
        private Mock<UserManager<User>> _userManager;
        private RegistrationUserDto _registerUser;

        public RegisterUserCommandHandlerTests()
        {
            _registerUser = new RegistrationUserDto
            {
                UserName = "TestUsername",
                Password = "Test",
                Email = "test@test.ru",
                Roles = new string[] { "User", "Manager" }
            };

            var storeUserManager = new Mock<IUserStore<User>>();
            _userManager = new Mock<UserManager<User>>(storeUserManager.Object, null, null, null, null, null, null, null, null);

            _registerUserCommand = new RegisterUserCommand(_registerUser);
            _registerUserCommandHandler = new RegisterUserCommandHandler(_userManager.Object);

            SetBaseSetups();
        }

        private void SetBaseSetups()
        {
            _userManager.Setup(x =>
                x.CreateAsync(It.IsAny<User>(), _registerUser.Password)).ReturnsAsync(IdentityResult.Success);
            _userManager.Setup(x =>
                x.AddToRolesAsync(It.IsAny<User>(), _registerUser.Roles)).ReturnsAsync(IdentityResult.Success);
        }

        [Fact]
        public async void Handle_ValidRegisterDto_SuccesfullIdentityResult()
        {
            var handleResult = await _registerUserCommandHandler.Handle(
                _registerUserCommand, CancellationToken.None);

            Assert.True(handleResult.Succeeded);
        }

        [Fact]
        public async void Handle_ErrorCreatingUser_FailedIdentityResult()
        {
            _userManager.Setup(x => x.CreateAsync(It.IsAny<User>(), _registerUser.Password))
                .ReturnsAsync(IdentityResult.Failed(new IdentityError()));

            var handleResult = await _registerUserCommandHandler.Handle(
                _registerUserCommand, CancellationToken.None);

            Assert.False(handleResult.Succeeded);
        }

        [Fact]
        public async void Handle_ErrorAddRoles_FailedIdentityResult()
        {
            _userManager.Setup(x =>
                x.CreateAsync(It.IsAny<User>(), _registerUser.Password)).ReturnsAsync(IdentityResult.Success);
            _userManager.Setup(x => x.AddToRolesAsync(It.IsAny<User>(), _registerUser.Roles))
                .ReturnsAsync(IdentityResult.Failed(new IdentityError()));

            var handleResult = await _registerUserCommandHandler.Handle(
                _registerUserCommand, CancellationToken.None);

            Assert.False(handleResult.Succeeded);
        }
    }
}
