﻿using AuthenticationService.Application.Commands.Abstractions;
using AuthenticationService.Contracts.Incoming;
using AuthenticationService.Contracts.Outgoing.Abstractions;
using AuthenticationService.Domain.Models;
using AuthenticationService.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Threading;
using System.Threading.Tasks;

namespace AuthenticationService.Application.Commands
{
    public class ChangeUserPasswordCommand : BaseCommand<ChangeUserPasswordDto, Response>
    {
        public ChangeUserPasswordCommand(ChangeUserPasswordDto changePassword) : base(changePassword) { }
    }

    class ChangeUserPasswordCommandHandler : IRequestHandler<ChangeUserPasswordCommand, Response>
    {
        private readonly IAuthenticationManager _authenticationManager;
        private readonly UserManager<User> _userManager;

        public ChangeUserPasswordCommandHandler(
            IAuthenticationManager authenticationManager,
            UserManager<User> userManager)
        {
            _authenticationManager = authenticationManager;
            _userManager = userManager;
        }

        public async Task<Response> Handle(ChangeUserPasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByNameAsync(request.Entity.Username);

            var result = await _userManager.ChangePasswordAsync(
                user, request.Entity.OldPassword, request.Entity.NewPassword);

            if (result.Succeeded == false)
            {
                return Response.Error;
            }

            return Response.Successfull;
        }
    }
}
