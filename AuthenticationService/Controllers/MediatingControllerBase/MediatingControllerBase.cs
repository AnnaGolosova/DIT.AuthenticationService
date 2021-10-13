using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
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
            string notFoundMessage = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (query == null)
                throw new ArgumentNullException(nameof(query));

            if (!ModelState.IsValid)
                return BadRequest("Invalid data provided");

            TResult response = await _mediator.Send(query, cancellationToken);

            if (response == null)
            {
                var actualNotFoundMessage = string.IsNullOrWhiteSpace(notFoundMessage)
                    ? string.Format("Not Found")
                    : notFoundMessage;

                return NotFound(actualNotFoundMessage);
            }

            return Ok(response);
        }

        protected async Task<IActionResult> ExecuteCommandAsync<TResult>(IRequest<TResult> command,
            string notFoundMessage = null, CancellationToken cancellationToken = default)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            if (!ModelState.IsValid)
                return BadRequest("Invalid data provided");
            try
            {
                TResult response = await _mediator.Send(command, cancellationToken);
                if (response == null)
                    throw new Exception("Error processing request");

                return Ok(response);
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Wrong username or password"))
                    return NotFound("Wrong username or password");

                return BadRequest(ex.Message);
            }
        }
    }
}
