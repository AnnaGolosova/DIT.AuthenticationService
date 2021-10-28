using AuthenticationService.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace AuthenticationService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public abstract class MediatingControllerBase : ControllerBase
    {
        private readonly IMediator _mediator;

        protected MediatingControllerBase(IMediator mediator)
        {
            _mediator = mediator;
        }

        protected IActionResult InternalServerError()
        {
            return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
        }

        protected async Task<IActionResult> ExecuteQueryAsync<TResult>(IRequest<TResult> query,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (query == null)
            {
                throw new ArgumentNullException(nameof(query));
            }

            if (!ModelState.IsValid)
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return BadRequest(allErrors);
            }

            try
            {
                TResult response = await _mediator.Send(query, cancellationToken);

                if (response == null)
                {
                    return InternalServerError();
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Wrong username or password"))
                {
                    return NotFound("Wrong username or password");
                }
                else if (ex.Message.Contains("Invalid connection to DB"))
                {
                    return InternalServerError();
                }

                return BadRequest(ex.Message);
            }
        }

        protected async Task<IActionResult> ExecuteCommandAsync<TResult>(IRequest<TResult> command,
            CancellationToken cancellationToken = default)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            if (!ModelState.IsValid)
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return BadRequest(allErrors);
            }

            try
            {
                TResult response = await _mediator.Send(command, cancellationToken);
                if ((response as IdentityResult).Succeeded == false)
                {
                    return BadRequest(response);
                }

                if (response == null)
                {
                    return InternalServerError();
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Wrong username or password"))
                {
                    return NotFound("Wrong username or password");
                }
                else if (ex.Message.Contains("Invalid connection to DB"))
                {
                    return InternalServerError();
                }

                return BadRequest(ex.Message);
            }
        }
    }
}
