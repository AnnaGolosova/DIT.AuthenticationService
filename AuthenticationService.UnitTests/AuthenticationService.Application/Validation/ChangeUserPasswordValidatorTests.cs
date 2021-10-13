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
    public class ChangeUserPasswordValidatorTests
    {
        private ValidationBehavior<ChangeUserPasswordCommand, IdentityResult> _validationBehavior;
        private Mock<RequestHandlerDelegate<IdentityResult>> _next;
        private Mock<ChangeUserPasswordCommand> _changeUserPasswordCommand;
        private Mock<IValidationConditions> _validateConditions;
        private ChangeUserPasswordDto _changePassword;

        public ChangeUserPasswordValidatorTests()
        {
            _changePassword = new ChangeUserPasswordDto()
            {
                UserName = "Test",
                OldPassword = "OldPassword1",
                NewPassword = "NewPassword1"
            };
            _changeUserPasswordCommand = new Mock<ChangeUserPasswordCommand>(_changePassword);
            _validateConditions = new Mock<IValidationConditions>();
            var authenticationValidator = new List<ChangeUserPasswordValidator>()
            {
                new ChangeUserPasswordValidator(_validateConditions.Object)
            };

            _validationBehavior = new ValidationBehavior<ChangeUserPasswordCommand, IdentityResult>(
                authenticationValidator);
            _next = new Mock<RequestHandlerDelegate<IdentityResult>>();

            SetBaseSetups();
        }

        private void SetBaseSetups()
        {
            _validateConditions.Setup(x => x.IsNotNullOrWhitespace(_changePassword.UserName)).Returns(true);
            _validateConditions.Setup(x => x.IsValidPassword(_changePassword.OldPassword)).Returns(true);
            _validateConditions.Setup(x => x.IsValidPassword(_changePassword.NewPassword)).Returns(true);
            _validateConditions.Setup(x => x.IsValidAuthenticate(_changePassword)).Returns(true);
        }

        [Fact]
        public async void Handle_InvalidUsername_ThrowExceptionWithMessage()
        {
            _validateConditions.Setup(x => x.IsNotNullOrWhitespace(_changePassword.UserName)).Returns(false);

            try
            {
                await _validationBehavior.Handle(_changeUserPasswordCommand.Object,
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
        public async void Handle_InvalidOldPassword_ThrowExceptionWithMessage()
        {
            _validateConditions.Setup(x => x.IsValidPassword(_changePassword.OldPassword)).Returns(false);

            try
            {
                await _validationBehavior.Handle(_changeUserPasswordCommand.Object,
                                                                    CancellationToken.None,
                                                                    _next.Object);
            }
            catch (Exception ex)
            {
                var exceptedExceptionMessage = "Old password must contain upper letter and digit";
                Assert.Equal(exceptedExceptionMessage, ex.Message);
            }
        }

        [Fact]
        public async void Handle_InvalidNewPassword_ThrowExceptionWithMessage()
        {
            _validateConditions.Setup(x => x.IsValidPassword(_changePassword.NewPassword)).Returns(false);

            try
            {
                await _validationBehavior.Handle(_changeUserPasswordCommand.Object,
                                                                    CancellationToken.None,
                                                                    _next.Object);
            }
            catch (Exception ex)
            {
                var exceptedExceptionMessage = "New password must contain upper letter and digit";
                Assert.Equal(exceptedExceptionMessage, ex.Message);
            }
        }

        [Fact]
        public async void Handle_UserNotExist_ThrowExceptionWithMessage()
        {
            _validateConditions.Setup(x => x.IsValidAuthenticate(_changePassword)).Returns(false);

            try
            {
                await _validationBehavior.Handle(_changeUserPasswordCommand.Object,
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
            _validateConditions.Setup(x => x.IsValidAuthenticate(_changePassword)).Returns(false);
            _validateConditions.Setup(x => x.IsNotNullOrWhitespace(_changePassword.UserName)).Returns(false);
            _validateConditions.Setup(x => x.IsValidPassword(_changePassword.OldPassword)).Returns(false);
            _validateConditions.Setup(x => x.IsValidPassword(_changePassword.NewPassword)).Returns(false);

            try
            {
                await _validationBehavior.Handle(_changeUserPasswordCommand.Object,
                                                                    CancellationToken.None,
                                                                    _next.Object);
            }
            catch (Exception ex)
            {
                var exceptedExceptionMessage = "Username is required field\n\r" +
                    "Old password must contain upper letter and digit\n\r" +
                    "New password must contain upper letter and digit\n\r" +
                    "Wrong username or password";
                Assert.Equal(exceptedExceptionMessage, ex.Message);
            }
        }
    }
}