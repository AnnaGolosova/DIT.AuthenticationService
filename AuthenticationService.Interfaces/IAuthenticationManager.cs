using AuthenticationService.Contracts.Incoming;
using System;
using System.Threading.Tasks;

namespace AuthenticationService.Interfaces
{
    public interface IAuthenticationManager
    {
        public Task<bool> ValidateUser(UserAuthenticationDto userForAuth);

        public Task<string> CreateToken();
    }
}
