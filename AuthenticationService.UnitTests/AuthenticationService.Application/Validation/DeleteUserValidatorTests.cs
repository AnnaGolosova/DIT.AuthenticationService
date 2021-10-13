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
    public class DeleteUserValidatorTests
    {
        private ValidationBehavior<DeleteUserCommand, IdentityResult> _validationBehavior;
        private Mock<RequestHandlerDelegate<IdentityResult>> _next;
        private Mock<DeleteUserCommand> _deleteUserCommand;
        private Mock<IValidationConditions> _validateConditions;
        private AuthenticationUserDto _authenticationUser;

        public DeleteUserValidatorTests()
        {
            _authenticationUser = new AuthenticationUserDto() { UserName = "Test", Password = "Password1" };
            _deleteUserCommand = new Mock<DeleteUserCommand>(_authenticationUser);
            _validateConditions = new Mock<IValidationConditions>();
            var authenticationValidator = new List<DeleteUserValidator>()
            {
                new DeleteUserValidator(_validateConditions.Object)
            };

            _validationBehavior = new ValidationBehavior<DeleteUserCommand, IdentityResult>(
                authenticationValidator);
            _next = new Mock<RequestHandlerDelegate<IdentityResult>>();

            SetBaseSetups();
        }

        private void SetBaseSetups()
        {
            _validateConditions.Setup(x => x.IsValidPassword(_authenticationUser.Password)).Returns(true);
            _validateConditions.Setup(x => x.IsNotNullOrWhitespace(_authenticationUser.UserName)).Returns(true);
            _validateConditions.Setup(x => x.IsValidAuthenticate(_authenticationUser)).Returns(true);
        }

        [Fact]
        public void Handle_ValidEntity_InvokeNext()
        {
            _validationBehavior.Handle(_deleteUserCommand.Object,
                                       CancellationToken.None,
                                       _next.Object);
            _next.Verify(x => x.Invoke());
        }

        [Fact]
        public async void Handle_NullEntity_ThrowExceptionWithMessage()
        {

            IdentityResult resultValidation;
            try
            {
                resultValidation = await _validationBehavior.Handle(_deleteUserCommand.Object,
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
        public async void Handle_InvalidUsername_ThrowExceptionWithMessage()
        {
            _validateConditions.Setup(x => x.IsNotNullOrWhitespace(_authenticationUser.UserName)).Returns(false);

            IdentityResult resultValidation;
            try
            {
                resultValidation = await _validationBehavior.Handle(_deleteUserCommand.Object,
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
            _validateConditions.Setup(x => x.IsValidPassword(_authenticationUser.Password)).Returns(false);

            IdentityResult resultValidation;
            try
            {
                resultValidation = await _validationBehavior.Handle(_deleteUserCommand.Object,
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
        public async void Handle_UserNotExist_ThrowExceptionWithMessage()
        {
            _validateConditions.Setup(x => x.IsValidAuthenticate(_authenticationUser)).Returns(false);

            IdentityResult resultValidation;
            try
            {
                resultValidation = await _validationBehavior.Handle(_deleteUserCommand.Object,
                                                                    CancellationToken.None,
                                                                    _next.Object);
            }
            catch (Exception ex)
            {
                var exceptedExceptionMessage = "Wrong username or password";
                Assert.Equal(exceptedExceptionMessage, ex.Message);
            }
        }
        [Fact]
        public async void Handle_AllInvalid_ThrowExceptionWithMessage()
        {
            _validateConditions.Setup(x => x.IsValidAuthenticate(_authenticationUser)).Returns(false);
            _validateConditions.Setup(x => x.IsNotNullOrWhitespace(_authenticationUser.UserName)).Returns(false);
            _validateConditions.Setup(x => x.IsValidPassword(_authenticationUser.Password)).Returns(false);

            IdentityResult resultValidation;
            try
            {
                resultValidation = await _validationBehavior.Handle(_deleteUserCommand.Object,
                                                                    CancellationToken.None,
                                                                    _next.Object);
            }
            catch (Exception ex)
            {
                var exceptedExceptionMessage = "Username is required field\n\r" +
                    "Password must contain upper letter and digit\n\r" +
                    "Wrong username or password";
                Assert.Equal(exceptedExceptionMessage, ex.Message);
            }
        }
    }
}
