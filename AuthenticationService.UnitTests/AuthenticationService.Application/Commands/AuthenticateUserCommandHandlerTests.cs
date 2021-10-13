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
    public class AuthenticateUserCommandHandlerTests
    {
        private AuthenticateUserCommandHandler _authenticateCommandHandler;
        private AuthenticateUserCommand _authenticateCommand;
        private Mock<IAuthenticationManager> _authenticationManager;
        private Mock<UserManager<User>> _userManager;
        private AuthenticationUserDto _authenticationUser;
        private User _user;

        public AuthenticateUserCommandHandlerTests()
        {
            _user = new User() { UserName = "TestUsername", Id = "TestId" };
            _authenticationUser = new AuthenticationUserDto { UserName = "TestUsername", Password = "Test" };

            var storeUserManager = new Mock<IUserStore<User>>();
            _userManager = new Mock<UserManager<User>>(storeUserManager.Object, null, null, null, null, null, null, null, null);
            _authenticationManager = new Mock<IAuthenticationManager>();

            _authenticateCommand = new AuthenticateUserCommand(_authenticationUser);

            _authenticateCommandHandler = new AuthenticateUserCommandHandler(
                _authenticationManager.Object,
                _userManager.Object);

            SetBaseSetups();
        }

        private void SetBaseSetups()
        {
            _userManager.Setup(x => x.FindByNameAsync(_user.UserName)).ReturnsAsync(_user);
            _userManager.Setup(x => x.GetRolesAsync(_user)).ReturnsAsync(new string[] { "User", "Moderator" });
            _authenticationManager.Setup(x => x.CreateToken()).ReturnsAsync("TestToken");
        }

        [Fact]
        public async void Handle_UserExists_ValidTokenWithUserRoles()
        {
            var handleResult = await _authenticateCommandHandler.Handle(_authenticateCommand, CancellationToken.None);

            var exceptedToken = "TestToken";
            var exceptedRoles = new List<string>() { "User", "Moderator" };

            Assert.Equal(exceptedRoles, handleResult.Roles.ToList());
            Assert.Equal(exceptedToken, handleResult.Token);
        }

        [Fact]
        public async void Handle_UserNotFound_NullValue()
        {
            _userManager.Setup(x => x.FindByNameAsync(_user.UserName)).ReturnsAsync(value: null);

            var handleResult = await _authenticateCommandHandler.Handle(_authenticateCommand, CancellationToken.None);

            Assert.Null(handleResult);
        }

        [Fact]
        public async void Handle_RolesNull_EntityWithNullRoles()
        {
            _userManager.Setup(x => x.GetRolesAsync(_user)).ReturnsAsync(value: null);

            var handleResult = await _authenticateCommandHandler.Handle(_authenticateCommand, CancellationToken.None);

            var exceptedToken = "TestToken";

            Assert.Null(handleResult.Roles);
            Assert.Equal(exceptedToken, handleResult.Token);
        }
    }
}
