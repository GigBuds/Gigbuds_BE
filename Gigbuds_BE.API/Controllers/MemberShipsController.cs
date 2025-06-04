using Gigbuds_BE.Application.Features.Memberships.Commands;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Gigbuds_BE.API.Controllers
{
    public class MemberShipsController : _BaseApiController
    {
        private readonly IMediator _mediator;

        public MemberShipsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterMembership(RegisterMembershipCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}
