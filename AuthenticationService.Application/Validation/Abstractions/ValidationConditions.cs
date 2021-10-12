using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;

namespace AuthenticationService.Application.Validation.Abstractions
{
    public static class ValidationConditions
    {
        public static bool IsNotNullOrWhitespace(string value) =>
            !string.IsNullOrWhiteSpace(value);

        public static bool IsValidPassword(string password) =>
            IsNotNullOrWhitespace(password) &&
            password.Any(char.IsDigit) &&
            password.Any(char.IsUpper) &&
           !password.Any(char.IsWhiteSpace);

        public static bool IsValidEmail(string email)
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

        public static bool IsValidRoles(ICollection<string> roles)
        {
            var existRoles = new List<string>() { "User", "Moderator", "Administrator" };
            var notExistUserRoles = roles.Where(role => !existRoles.Contains(role)).ToList();

            if (notExistUserRoles.Count > 0)
            {
                return false;
            }

            return true;
        }
    }
}
