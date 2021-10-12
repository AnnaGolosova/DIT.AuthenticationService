using Microsoft.AspNetCore.Identity;
using System;

namespace AuthenticationService.Domain.Models
{
    public class User : IdentityUser
    {
        public Guid ProfileId { get; set; }
    }
}
