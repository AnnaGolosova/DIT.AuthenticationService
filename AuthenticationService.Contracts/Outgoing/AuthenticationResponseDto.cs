﻿using System.Collections.Generic;

namespace AuthenticationService.Contracts.Outgoing
{
    public class AuthenticationResponseDto
    {
        public string Token { get; set; }

        public ICollection<string> Roles { get; set; }
    }
}
