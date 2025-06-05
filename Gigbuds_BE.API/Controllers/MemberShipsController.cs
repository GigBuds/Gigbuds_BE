using Gigbuds_BE.Application.Features.Memberships.Commands;
using Gigbuds_BE.Application.Interfaces.Services;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gigbuds_BE.API.Controllers
{
    public class MemberShipsController : _BaseApiController
    {
        private readonly IMediator _mediator;
        private readonly IMembershipsService _membershipsService;

        public MemberShipsController(IMediator mediator, IMembershipsService membershipsService)
        {
            _mediator = mediator;
            _membershipsService = membershipsService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterMembership(RegisterMembershipCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        /// <summary>
        /// Revokes a membership for a specific user (Admin/Staff only)
        /// </summary>
        /// <param name="accountId">The ID of the account</param>
        /// <param name="membershipId">The ID of the membership to revoke</param>
        /// <returns>Success or error response</returns>
        [HttpDelete("revoke/{accountId}/{membershipId}")]
        //[Authorize(Roles = "Admin,Staff")]
 
        public async Task<IActionResult> RevokeMembership(int accountId, int membershipId)
        {
            try
            {
                await _membershipsService.RevokeMembershipAsync(accountId, membershipId);
                return Ok(new { 
                    message = "Membership revoked successfully",
                    accountId = accountId,
                    membershipId = membershipId,
                    revokedAt = DateTime.UtcNow
                });
            }
            catch (Exception ex) when (ex.Message.Contains("Account not found"))
            {
                return NotFound(new { error = $"Account with ID {accountId} not found" });
            }
            catch (Exception ex) when (ex.Message.Contains("not found"))
            {
                return NotFound(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
