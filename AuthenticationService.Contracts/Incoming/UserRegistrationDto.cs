using System;
using System.Collections.Generic;
using System.Text;

namespace AuthenticationService.Contracts.Incoming
{
    public class UserRegistrationDto
    {
        public string Username { get; set; }

        public string Email { get; set; }

        public ICollection<string> Roles { get; set; }
    }
}
