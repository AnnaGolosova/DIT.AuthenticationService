using AuthenticationService.Application.Commands.Abstractions;
using AuthenticationService.Contracts.Incoming;
using AuthenticationService.Contracts.Outgoing.Abstractions;
using AuthenticationService.Domain.Models;
using AuthenticationService.Interfaces;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Threading;
using System.Threading.Tasks;

namespace AuthenticationService.Application.Commands
{
    public class RegisterUserCommand : BaseCommand<RegistrationUserDto, Response>
    {
        public RegisterUserCommand(RegistrationUserDto changePassword) : base(changePassword) { }
    }

    class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, Response>
    {
        private readonly IAuthenticationManager _authenticationManager;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;

        public RegisterUserCommandHandler(
            IAuthenticationManager authenticationManager,
            UserManager<User> userManager,
            IMapper mapper)
        {
            _authenticationManager = authenticationManager;
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<Response> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var userForRegistration = request.Entity;

            var user = _mapper.Map<User>(userForRegistration);

            await _userManager.AddToRolesAsync(user, userForRegistration.Roles);

            return Response.Successfull;
        }
    }
}


