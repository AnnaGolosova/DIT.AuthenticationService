using AuthenticationService.Contracts.Incoming;
using AuthenticationService.Domain.Models;
using AutoMapper;

namespace AuthenticationService.Contracts
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<RegistrationUserDto, AuthenticationUserDto>();

            CreateMap<RegistrationUserDto, User>();

            CreateMap<AuthenticationUserDto, User>();
        }
    }
}
