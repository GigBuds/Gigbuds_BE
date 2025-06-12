using Gigbuds_BE.Application.Features.Memberships.Commands;
using Gigbuds_BE.Application.Features.Memberships.Commands.CreateMembershipPayment;
using Gigbuds_BE.Application.Features.Memberships.Queries;
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
        /// Creates a payment link for membership registration using PayOS
        /// Supports both web and mobile applications
        /// </summary>
        /// <param name="command">Membership payment command</param>
        /// <returns>Payment link with checkout URL and QR code</returns>
        [HttpPost("payment")]
        [Authorize]
        public async Task<IActionResult> CreateMembershipPayment(CreateMembershipPaymentCommand command)
        {
            try
            {
                var result = await _mediator.Send(command);
                return Ok(new
                {
                    success = true,
                    data = result,
                    message = "Payment link created successfully. Use checkoutUrl for web or qrCode for mobile payments."
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { success = false, error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, error = "Internal server error" });
            }
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

        [HttpGet("all")]
        public async Task<IActionResult> GetAllMembership()
        {
            var query = new GetAllMembershipQuery();
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("all/{accountId}")]
        public async Task<IActionResult> GetAllMembershipByAccountId(int accountId) {
            var query = new GetAllMembershipByAccountIdQuery(accountId);
            var result = await _mediator.Send(query);
            return Ok(result);
        }
    }
}
