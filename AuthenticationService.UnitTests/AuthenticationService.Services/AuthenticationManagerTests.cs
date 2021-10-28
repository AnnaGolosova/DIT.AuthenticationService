using AuthenticationService.Contracts.Incoming;
using AuthenticationService.Domain.Models;
using AuthenticationService.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Moq;
using Xunit;

namespace AuthenticationService.UnitTests.AuthenticationService.Services
{
    public class AuthenticationManagerTests
    {
        private Mock<IConfiguration> _configuration = new Mock<IConfiguration>();
        private Mock<UserManager<User>> _userManager;
        private AuthenticationManager _authManager;

        public AuthenticationManagerTests()
        {
            SetUserManager();
            _authManager = new AuthenticationManager(_userManager.Object, _configuration.Object);
            _configuration.Setup(x => x.GetSection("SECRET").Value).Returns("testSecret");
            _configuration.Setup(x => x.GetSection("JwtSettings").Value).Returns("\"validIssuer\":\"ProductsApi\",\"validAudience\":\"https://localhost:5001\",\"expires\":30");
        }

        private void SetUserManager()
        {
            var store = new Mock<IUserStore<User>>();
            _userManager = new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);
            _userManager.Setup(x => x.FindByNameAsync(It.IsAny<string>())).ReturnsAsync(new User() { UserName = "UserName", Id = "id" });
            _userManager.Setup(x => x.CheckPasswordAsync(It.IsAny<User>(), It.IsAny<string>())).ReturnsAsync(true);
            _userManager.Setup(x => x.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(new string[] { "Manager", "User" });
        }

        [Fact]
        public async void ValidateUser_ValidAuthenticationUserDto_True()
        {
            var result = await _authManager.ValidateUser(new AuthenticationUserDto());

            Assert.True(result);
        }

        [Fact]
        public async void ValidateUser_UserNotExists_False()
        {
            _userManager.Setup(x => x.FindByNameAsync(It.IsAny<string>())).ReturnsAsync(value: null);

            var result = await _authManager.ValidateUser(new AuthenticationUserDto());

            Assert.False(result);
        }

        [Fact]
        public async void ValidateUser_InvalidPassword_False()
        {
            _userManager.Setup(x => x.CheckPasswordAsync(It.IsAny<User>(), It.IsAny<string>())).ReturnsAsync(false);

            var result = await _authManager.ValidateUser(new AuthenticationUserDto());

            Assert.False(result);
        }
    }
}
