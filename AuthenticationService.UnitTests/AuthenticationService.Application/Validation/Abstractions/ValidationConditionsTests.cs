using AuthenticationService.Application.Validation.Abstractions;
using AuthenticationService.Domain.Models;
using AuthenticationService.Interfaces;
using Microsoft.AspNetCore.Identity;
using Moq;
using Xunit;

namespace AuthenticationService.UnitTests.AuthenticationService.Application.Validation.Abstractions
{
    public class ValidationConditionsTests
    {
        private readonly ValidationConditions _validationConditions;

        public ValidationConditionsTests()
        {
            var storeUserManager = new Mock<IUserStore<User>>();
            var userManager = new Mock<UserManager<User>>(storeUserManager.Object, null, null, null, null, null, null, null, null);
            var storeRoleManager = new Mock<IRoleStore<IdentityRole>>();
            var roleManager = new Mock<RoleManager<IdentityRole>>(storeRoleManager.Object, null, null, null, null);
            var authenticationManager = new Mock<IAuthenticationManager>();

            _validationConditions = new ValidationConditions(
                authenticationManager.Object,
                roleManager.Object,
                userManager.Object);
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        public void IsNotNullOrWhitespace_InvalidString_False(string inputValue)
        {
            var methodResult = _validationConditions.IsNotNullOrWhitespace(inputValue);

            Assert.False(methodResult);
        }

        [Theory]
        [InlineData("c")]
        [InlineData("  test  ")]
        [InlineData("   leftSpaces")]
        [InlineData("rightSpaces   ")]
        public void IsNotNullOrWhitespace_FilledString_True(string inputValue)
        {
            var methodResult = _validationConditions.IsNotNullOrWhitespace(inputValue);

            Assert.True(methodResult);
        }

        [Theory]
        [InlineData("")]
        [InlineData("short")]
        [InlineData("Sh1")]
        [InlineData("PassWithoutDigit")]
        [InlineData("pass0without0upper")]
        public void IsValidPassword_InvalidPassword_False(string inputValue)
        {
            var methodResult = _validationConditions.IsValidPassword(inputValue);

            Assert.False(methodResult);
        }

        [Theory]
        [InlineData("Password1")]
        [InlineData("UPPERCASE123")]
        [InlineData("LongValidPassword123")]
        public void IsValidPassword_ValidPassword_True(string inputValue)
        {
            var methodResult = _validationConditions.IsValidPassword(inputValue);

            Assert.True(methodResult);
        }
    }
}
