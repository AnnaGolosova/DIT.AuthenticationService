using AuthenticationService.Application.Commands;
using AuthenticationService.Contracts.Incoming;
using AuthenticationService.Domain.Models;
using AuthenticationService.Interfaces;
using Microsoft.AspNetCore.Identity;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Xunit;

namespace AuthenticationService.UnitTests.AuthenticationService.Application.Commands
{
    public class DeleteUserCommandHandlerTests
    {
        private DeleteUserCommandHandler _deleteUserCommandHandler;
        private DeleteUserCommand _deleteUserCommand;
        private Mock<IAuthenticationManager> _authenticationManager;
        private Mock<UserManager<User>> _userManager;
        private AuthenticationUserDto _authenticationUser;
        private User _user;

        public DeleteUserCommandHandlerTests()
        {
            _user = new User() { UserName = "TestUsername", Id = "TestId" };
            _authenticationUser = new AuthenticationUserDto { UserName = "TestUsername", Password = "Test" };

            var storeUserManager = new Mock<IUserStore<User>>();
            _userManager = new Mock<UserManager<User>>(storeUserManager.Object, null, null, null, null, null, null, null, null);
            _authenticationManager = new Mock<IAuthenticationManager>();

            _deleteUserCommand = new DeleteUserCommand(_authenticationUser);

            _deleteUserCommandHandler = new DeleteUserCommandHandler(
                _authenticationManager.Object,
                _userManager.Object);

            SetBaseSetups();
        }

        private void SetBaseSetups()
        {
            _userManager.Setup(x => x.FindByNameAsync(_user.UserName)).ReturnsAsync(_user);
            _userManager.Setup(x => x.DeleteAsync(_user)).ReturnsAsync(IdentityResult.Success);
        }

        [Fact]
        public async void Handle_UserExistAndValidPassword_SuccesfullIdentityResult()
        {
            var handleResult = await _deleteUserCommandHandler.Handle(
                _deleteUserCommand, CancellationToken.None);

            Assert.True(handleResult.Succeeded);
        }

        [Fact]
        public async void Handle_InvalidPassword_FailedIdentityResult()
        {
            _userManager.Setup(x => x.DeleteAsync(_user)).
                ReturnsAsync(IdentityResult.Failed(new IdentityError()));

            var handleResult = await _deleteUserCommandHandler.Handle(
                _deleteUserCommand, CancellationToken.None);

            Assert.False(handleResult.Succeeded);
        }

        [Fact]
        public async void Handle_UserNotFound_FailedIdentityResult()
        {
            _userManager.Setup(x => x.DeleteAsync(_user)).
                ReturnsAsync(IdentityResult.Failed(new IdentityError()));

            var handleResult = await _deleteUserCommandHandler.Handle(
                _deleteUserCommand, CancellationToken.None);

            Assert.False(handleResult.Succeeded);
        }
    }
}
