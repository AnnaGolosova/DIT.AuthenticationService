using AuthenticationService.Application.Commands;
using AuthenticationService.Contracts.Incoming;
using AuthenticationService.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Moq;
using System.Threading;
using Xunit;

namespace AuthenticationService.UnitTests.AuthenticationService.Application.Commands
{
    public class ChangeUserPasswordCommandTests
    {
        private ChangeUserPasswordCommandHandler _changeUserPasswordCommandHandler;
        private ChangeUserPasswordCommand _changeUserPasswordCommand;
        private Mock<UserManager<User>> _userManager;
        private ChangeUserPasswordDto _changePassword;
        private User _user;

        public ChangeUserPasswordCommandTests()
        {
            _user = new User() { UserName = "TestUsername", Id = "TestId" };
            _changePassword = new ChangeUserPasswordDto
            {
                UserName = "TestUsername",
                OldPassword = "TestOldPassword",
                NewPassword = "TestNewPassword"
            };

            var storeUserManager = new Mock<IUserStore<User>>();
            _userManager = new Mock<UserManager<User>>(storeUserManager.Object, null, null, null, null, null, null, null, null);

            _changeUserPasswordCommand = new ChangeUserPasswordCommand(_changePassword);

            _changeUserPasswordCommandHandler = new ChangeUserPasswordCommandHandler(
                _userManager.Object);

            SetBaseSetups();
        }

        private void SetBaseSetups()
        {
            _userManager.Setup(x => x.FindByNameAsync(_changePassword.UserName)).ReturnsAsync(_user);
            _userManager.Setup(x => x.ChangePasswordAsync(
                _user,
                _changePassword.OldPassword,
                _changePassword.NewPassword)
            ).ReturnsAsync(IdentityResult.Success);
        }

        [Fact]
        public async void Handle_UserExistAndValidPassword_SuccesfullIdentityResult()
        {
            var handleResult = await _changeUserPasswordCommandHandler.Handle(
                _changeUserPasswordCommand, CancellationToken.None);

            Assert.True(handleResult.Succeeded);
        }

        [Fact]
        public async void Handle_InvalidOldPassword_FailedIdentityResult()
        {
            _userManager.Setup(x => x.ChangePasswordAsync(
                _user,
                _changePassword.OldPassword,
                _changePassword.NewPassword)
            ).ReturnsAsync(IdentityResult.Failed(new IdentityError()));

            var handleResult = await _changeUserPasswordCommandHandler.Handle(
                _changeUserPasswordCommand, CancellationToken.None);

            Assert.False(handleResult.Succeeded);
        }

        [Fact]
        public async void Handle_UserNotFound_FailedIdentityResult()
        {
            _userManager.Setup(x =>
                x.FindByNameAsync(_changePassword.UserName)).ReturnsAsync(value: null);
            _userManager.Setup(x => x.ChangePasswordAsync(
                null,
                _changePassword.OldPassword,
                _changePassword.NewPassword)
            ).ReturnsAsync(IdentityResult.Failed(new IdentityError()));

            var handleResult = await _changeUserPasswordCommandHandler.Handle(
                _changeUserPasswordCommand, CancellationToken.None);

            Assert.False(handleResult.Succeeded);
        }
    }
}
