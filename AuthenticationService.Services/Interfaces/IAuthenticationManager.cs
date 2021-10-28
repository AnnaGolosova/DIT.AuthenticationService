using AuthenticationService.Contracts.Incoming;
using System.Threading.Tasks;

namespace AuthenticationService.Interfaces
{
    public interface IAuthenticationManager
    {
        public Task<bool> ValidateUser(AuthenticationUserDto userForAuth);

        public Task<string> CreateToken();
    }
}
