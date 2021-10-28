using AuthenticationService.Application.Validation.Abstractions;
using AuthenticationService.Contracts.Incoming;
using AuthenticationService.Domain.Models;
using AuthenticationService.Interfaces;
using Microsoft.AspNetCore.Identity;
using Moq;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace AuthenticationService.UnitTests.AuthenticationService.Application.Validation.Abstractions
{
    public class ValidationConditionsTests
    {
        private Mock<RoleManager<IdentityRole>> _roleManager;
        private Mock<UserManager<User>> _userManager;
        private Mock<IAuthenticationManager> _authenticationManager;
        private readonly ValidationConditions _validationConditions;

        public ValidationConditionsTests()
        {
            var storeUserManager = new Mock<IUserStore<User>>();
            var storeRoleManager = new Mock<IRoleStore<IdentityRole>>();

            _userManager = new Mock<UserManager<User>>(storeUserManager.Object, null, null, null, null, null, null, null, null);
            _roleManager = new Mock<RoleManager<IdentityRole>>(storeRoleManager.Object, null, null, null, null);
            _authenticationManager = new Mock<IAuthenticationManager>();


            _validationConditions = new ValidationConditions(
                _authenticationManager.Object,
                _roleManager.Object,
                _userManager.Object);

            SetBaseSetups();
        }

        private void SetBaseSetups()
        {
            var user = new User() { UserName = "TestUsername", Id = "TestId" };
            _userManager.Setup(x => x.FindByNameAsync("TestUsername")).ReturnsAsync(user);

            _roleManager.Setup(x => x.Roles).Returns(
                new List<IdentityRole>() {
                    new IdentityRole(){Name = "User"},
                    new IdentityRole(){Name = "Moderator"},
                    new IdentityRole(){Name = "Administrator"},
                }.AsQueryable);
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

        [Theory]
        [InlineData("")]
        [InlineData(".asda@asd.asd")]
        [InlineData("example")]
        [InlineData("example.com")]
        [InlineData("#!$%&@’*+-/=?^_`{}|~@example.com")]
        public void IsValidEmail_InvalidEmail_False(string inputValue)
        {
            var methodResult = _validationConditions.IsValidEmail(inputValue);

            Assert.False(methodResult);
        }

        [Theory]
        [InlineData("test@test.ru")]
        [InlineData("TEST@TEST.RU")]
        [InlineData("#!$%&’*+-/=?^_`{}|~@example.com")]
        public void IsValidEmail_ValidEmail_True(string inputValue)
        {
            var methodResult = _validationConditions.IsValidEmail(inputValue);

            Assert.True(methodResult);
        }

        [Theory]
        [InlineData("User", "Teacher")]
        [InlineData("Used", "Moderatar", "Odministratar")]
        [InlineData("User", "Moderator", "Administrator", "Programmer")]
        public void IsValidRoles_InvalidRoles_False(params string[] inputValue)
        {
            var methodResult = _validationConditions.RolesExists(inputValue);

            Assert.False(methodResult);
        }

        [Theory]
        [InlineData("User")]
        [InlineData("User", "Moderator")]
        [InlineData("User", "Moderator", "Administrator")]
        [InlineData("User", "Administrator")]
        [InlineData("Administrator")]
        public void IsValidRoles_ValidRoles_True(params string[] inputValue)
        {
            var methodResult = _validationConditions.RolesExists(inputValue);

            Assert.True(methodResult);
        }

        [Fact]
        public void UserExists_ExistsUsername_True()
        {
            var inputValue = "TestUsername";

            var methodResult = _validationConditions.UserExists(inputValue);

            Assert.True(methodResult);
        }

        [Fact]
        public void UserExists_NotExistsUsername_False()
        {
            var inputValue = "NotExistUsername";

            var methodResult = _validationConditions.UserExists(inputValue);

            Assert.False(methodResult);
        }

        [Fact]
        public void IsValidAuthenticate_InvalidAuthenticationUserDto_False()
        {
            var inputValue = new AuthenticationUserDto()
            {
                UserName = "InvalidUsername",
                Password = "InvalidPassword"
            };

            _authenticationManager.Setup(x => x.ValidateUser(inputValue)).ReturnsAsync(false);

            var methodResult = _validationConditions.IsValidAuthenticate(inputValue);

            Assert.False(methodResult);
        }

        [Fact]
        public void IsValidAuthenticate_ValidAuthenticationUserDto_True()
        {
            var inputValue = new AuthenticationUserDto()
            {
                UserName = "ValidUsername",
                Password = "ValidPassword"
            };

            _authenticationManager.Setup(x => x.ValidateUser(inputValue)).ReturnsAsync(true);

            var methodResult = _validationConditions.IsValidAuthenticate(inputValue);

            Assert.True(methodResult);
        }

        [Fact]
        public void IsValidAuthenticate_InvalidChangeUserPasswordDto_False()
        {
            var inputValue = new ChangeUserPasswordDto()
            {
                UserName = "InvalidUsername",
                OldPassword = "InvalidOldPassword",
                NewPassword = "InvalidNewPassword"
            };

            _authenticationManager.Setup(x => x.ValidateUser(It.IsAny<AuthenticationUserDto>())).ReturnsAsync(false);

            var methodResult = _validationConditions.IsValidAuthenticate(inputValue);

            Assert.False(methodResult);
        }

        [Fact]
        public void IsValidAuthenticate_ValidChangeUserPasswordDto_True()
        {
            var inputValue = new ChangeUserPasswordDto()
            {
                UserName = "ValidUsername",
                OldPassword = "ValidOldPassword",
                NewPassword = "ValidNewPassword"
            };

            _authenticationManager.Setup(x => x.ValidateUser(It.IsAny<AuthenticationUserDto>())).ReturnsAsync(true);

            var methodResult = _validationConditions.IsValidAuthenticate(inputValue);

            Assert.True(methodResult);
        }
    }
}
