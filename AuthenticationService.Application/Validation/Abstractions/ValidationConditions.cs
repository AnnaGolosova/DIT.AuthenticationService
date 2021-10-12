using AuthenticationService.Application.Validation.Abstractions.Interfaces;
using AuthenticationService.Domain.Models;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;

namespace AuthenticationService.Application.Validation.Abstractions
{
    public class ValidationConditions : IValidationConditions
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public ValidationConditions(UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
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
    }
}
