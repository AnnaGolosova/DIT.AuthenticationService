using MediatR;

namespace AuthenticationService.Controllers
{
    public class AccountController : MediatingControllerBase
    {
        public AccountController(IMediator _mediator) : base(_mediator)
        { }
    }
}
