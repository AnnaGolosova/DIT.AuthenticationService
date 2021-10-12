using AuthenticationService.Application.Validation.Abstractions.Interfaces;
using AuthenticationService.Contracts.Incoming;
using AuthenticationService.Domain.Models;
using AuthenticationService.Interfaces;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;

namespace AuthenticationService.Application.Validation.Abstractions
{
    public class ValidationConditions : IValidationConditions
    {
        private readonly IAuthenticationManager _authenticationManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<User> _userManager;

        public ValidationConditions(
            IAuthenticationManager authenticationManager,
            RoleManager<IdentityRole> roleManager,
            UserManager<User> userManager)
        {
            _authenticationManager = authenticationManager;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public bool IsNotNullOrWhitespace(string value) =>
            !string.IsNullOrWhiteSpace(value);

        public bool IsValidPassword(string password) =>
            IsNotNullOrWhitespace(password) &&
            password.Any(char.IsDigit) &&
            password.Any(char.IsUpper) &&
           !password.Any(char.IsWhiteSpace);

        public bool IsValidEmail(string email)
        {
            try
            {
                var address = new MailAddress(email).Address;
                return address == email && IsNotNullOrWhitespace(address);
            }
            catch
            {
                return false;
            }
        }

        public bool RolesExists(ICollection<string> roles)
        {
            var existRoles = _roleManager.Roles.Select(r => r.Name).ToList();
            var notExistUserRoles = roles.Where(role => !existRoles.Contains(role)).ToList();

            if (notExistUserRoles.Count > 0)
            {
                return false;
            }

            return true;
        }

        public bool UserExists(string username)
        {
            var user = _userManager.FindByNameAsync(username).Result;
            return user != null;
        }

        public bool IsValidUser(AuthenticationUserDto user)
        {
            var validationResult = _authenticationManager.ValidateUser(user).Result;
            return validationResult;
        }

        public bool IsValidUser(ChangeUserPasswordDto changePassword)
        {
            var user = new AuthenticationUserDto()
            {
                Username = changePassword.Username,
                Password = changePassword.OldPassword
            };

            var validationResult = _authenticationManager.ValidateUser(user).Result;
            return validationResult;
        }
    }
}
