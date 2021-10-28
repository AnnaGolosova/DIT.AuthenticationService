using AuthenticationService.Application.Commands;
using AuthenticationService.Application.Validation;
using AuthenticationService.Application.Validation.Abstractions;
using AuthenticationService.Application.Validation.Abstractions.Interfaces;
using AuthenticationService.Contracts.Incoming;
using AuthenticationService.Contracts.Outgoing;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading;
using Xunit;

namespace AuthenticationService.UnitTests.AuthenticationService.Application.Validation
{
    public class RegistrationValidatorTests
    {
        private ValidationBehavior<RegisterUserCommand, IdentityResult> _validationBehavior;
        private Mock<RequestHandlerDelegate<IdentityResult>> _next;
        private Mock<RegisterUserCommand> _registerUserCommand;
        private Mock<IValidationConditions> _validateConditions;
        private RegistrationUserDto _registrationUser;

        public RegistrationValidatorTests()
        {
            _registrationUser = new RegistrationUserDto()
            {
                UserName = "Username",
                Email = "Test@Test.ru",
                Password = "Password1",
                Roles = new string[] { "User", "Moderator" }
            };
            _registerUserCommand = new Mock<RegisterUserCommand>(_registrationUser);
            _validateConditions = new Mock<IValidationConditions>();
            var authenticationValidator = new List<RegistrationValidator>()
            {
                new RegistrationValidator(_validateConditions.Object)
            };

            _validationBehavior = new ValidationBehavior<RegisterUserCommand, IdentityResult>(
                authenticationValidator);
            _next = new Mock<RequestHandlerDelegate<IdentityResult>>();

            SetBaseSetups();
        }

        private void SetBaseSetups()
        {
            _validateConditions.Setup(x => x.IsNotNullOrWhitespace(_registrationUser.UserName)).Returns(true);
            _validateConditions.Setup(x => x.IsValidPassword(_registrationUser.Password)).Returns(true);
            _validateConditions.Setup(x => x.IsValidEmail(_registrationUser.Email)).Returns(true);
            _validateConditions.Setup(x => x.RolesExists(_registrationUser.Roles)).Returns(true);
        }

        [Fact]
        public async void Handle_InvalidUsername_ThrowExceptionWithMessage()
        {
            _validateConditions.Setup(x => x.IsNotNullOrWhitespace(_registrationUser.UserName)).Returns(false);

            try
            {
                await _validationBehavior.Handle(_registerUserCommand.Object,
                                                                    CancellationToken.None,
                                                                    _next.Object);
            }
            catch (Exception ex)
            {
                var exceptedExceptionMessage = "Username is required field";
                Assert.Equal(exceptedExceptionMessage, ex.Message);
            }
        }

        [Fact]
        public async void Handle_InvalidPassword_ThrowExceptionWithMessage()
        {
            _validateConditions.Setup(x => x.IsValidPassword(_registrationUser.Password)).Returns(false);

            try
            {
                await _validationBehavior.Handle(_registerUserCommand.Object,
                                                                    CancellationToken.None,
                                                                    _next.Object);
            }
            catch (Exception ex)
            {
                var exceptedExceptionMessage = "Password must contain upper letter and digit";
                Assert.Equal(exceptedExceptionMessage, ex.Message);
            }
        }

        [Fact]
        public async void Handle_NotExistsRoles_ThrowExceptionWithMessage()
        {
            _validateConditions.Setup(x => x.RolesExists(_registrationUser.Roles)).Returns(false);

            try
            {
                await _validationBehavior.Handle(_registerUserCommand.Object,
                                                                    CancellationToken.None,
                                                                    _next.Object);
            }
            catch (Exception ex)
            {
                var exceptedExceptionMessage = "Invalid roles. Possible roles: Administrator, Moderator and User";
                Assert.Equal(exceptedExceptionMessage, ex.Message);
            }
        }

        [Fact]
        public async void Handle_AllInvalid_ThrowExceptionWithMessage()
        {
            _validateConditions.Setup(x => x.IsNotNullOrWhitespace(_registrationUser.UserName)).Returns(false);
            _validateConditions.Setup(x => x.IsValidPassword(_registrationUser.Password)).Returns(false);
            _validateConditions.Setup(x => x.IsValidEmail(_registrationUser.Email)).Returns(false);
            _validateConditions.Setup(x => x.RolesExists(_registrationUser.Roles)).Returns(false);

            try
            {
                await _validationBehavior.Handle(_registerUserCommand.Object,
                                                                    CancellationToken.None,
                                                                    _next.Object);
            }
            catch (Exception ex)
            {
                var exceptedExceptionMessage = "Username is required field\n\r" +
                    "Password must contain upper letter and digit\n\r" +
                    "Invalid email address\n\r" +
                    "Invalid roles. Possible roles: Administrator, Moderator and User";
                Assert.Equal(exceptedExceptionMessage, ex.Message);
            }
        }
    }
}
