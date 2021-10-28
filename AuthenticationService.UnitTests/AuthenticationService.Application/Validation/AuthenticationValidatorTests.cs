using AuthenticationService.Application.Commands;
using AuthenticationService.Application.Queries;
using AuthenticationService.Application.Validation;
using AuthenticationService.Application.Validation.Abstractions;
using AuthenticationService.Application.Validation.Abstractions.Interfaces;
using AuthenticationService.Contracts.Incoming;
using AuthenticationService.Contracts.Outgoing;
using MediatR;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading;
using Xunit;

namespace AuthenticationService.UnitTests.AuthenticationService.Application.Validation
{
    public class AuthenticationValidatorTests
    {
        private ValidationBehavior<AuthenticateUserQuery, AuthenticationResponse> _validationBehavior;
        private Mock<RequestHandlerDelegate<AuthenticationResponse>> _next;
        private Mock<AuthenticateUserQuery> _AuthenticateUserQuery;
        private Mock<IValidationConditions> _validateConditions;
        private AuthenticationUserDto _authenticationUser;

        public AuthenticationValidatorTests()
        {
            _authenticationUser = new AuthenticationUserDto() { UserName = "Test", Password = "Password1" };
            _AuthenticateUserQuery = new Mock<AuthenticateUserQuery>(_authenticationUser);
            _validateConditions = new Mock<IValidationConditions>();
            var authenticationValidator = new List<AuthenticationValidator>()
            {
                new AuthenticationValidator(_validateConditions.Object)
            };

            _validationBehavior = new ValidationBehavior<AuthenticateUserQuery, AuthenticationResponse>(
                authenticationValidator);
            _next = new Mock<RequestHandlerDelegate<AuthenticationResponse>>();

            SetBaseSetups();
        }

        private void SetBaseSetups()
        {
            _validateConditions.Setup(x => x.IsValidPassword(_authenticationUser.Password)).Returns(true);
            _validateConditions.Setup(x => x.IsNotNullOrWhitespace(_authenticationUser.UserName)).Returns(true);
            _validateConditions.Setup(x => x.IsValidAuthenticate(_authenticationUser)).Returns(true);
        }

        [Fact]
        public async void Handle_InvalidUsername_ThrowExceptionWithMessage()
        {
            _validateConditions.Setup(x => x.IsNotNullOrWhitespace(_authenticationUser.UserName)).Returns(false);

            try
            {
                await _validationBehavior.Handle(_AuthenticateUserQuery.Object,
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

            try
            {
                await _validationBehavior.Handle(_AuthenticateUserQuery.Object,
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

            try
            {
                await _validationBehavior.Handle(_AuthenticateUserQuery.Object,
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

            try
            {
                await _validationBehavior.Handle(_AuthenticateUserQuery.Object,
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
