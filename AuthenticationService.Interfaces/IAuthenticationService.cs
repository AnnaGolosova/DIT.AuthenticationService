using AuthenticationService.Contracts.Incoming;
using System;
using System.Threading.Tasks;

namespace AuthenticationService.Interfaces
{
    public interface IAuthenticationService
    {
        public Task<bool> ValidateUser(UserAuthenticationDto userForAuth);

        public Task<string> CreateToken();
    }
}
