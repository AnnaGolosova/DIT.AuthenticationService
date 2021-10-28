using System.Collections.Generic;

namespace AuthenticationService.Contracts.Outgoing
{
    public class AuthenticationResponseDto
    {
        public string UserId { get; set; }

        public string UserName { get; set; }

        public string AccessToken { get; set; }

        public ICollection<string> Roles { get; set; }
    }
}
