using AuthenticationService.Contracts.Incoming;
using System.Collections.Generic;

namespace AuthenticationService.Application.Validation.Abstractions.Interfaces
{
    public interface IValidationConditions
    {
        bool IsNotNullOrWhitespace(string value);

        bool IsValidEmail(string email);

        bool IsValidPassword(string password);

        bool RolesExists(ICollection<string> roles);

        bool UserExists(string username);

        bool IsValidUser(AuthenticationUserDto user);

        bool IsValidUser(ChangeUserPasswordDto user);
    }
}