using Gigbuds_BE.Application.DTOs.ApplicationUsers;
using Gigbuds_BE.Application.Features.Authentication.Commands.Login;
using Gigbuds_BE.Application.Features.Authentication.Commands.Register.RegisterForAdmin;
using Gigbuds_BE.Application.Features.Authentication.Commands.Register.RegisterForEmployer;
using Gigbuds_BE.Application.Features.Authentication.Commands.Register.RegisterForStaff;
using Gigbuds_BE.Application.Features.Authentication.Commands.Register.RegisterForUsers;
using Gigbuds_BE.Application.Features.Authentication.Commands.SendVerificationCode;
using Gigbuds_BE.Application.Features.Authentication.Commands.VerifyPhone;
using Gigbuds_BE.Domain.Entities.Identity;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Gigbuds_BE.API.Controllers
{
    public class IdentitiesController(IMediator mediator) : _BaseApiController
    {
        // POST /api/v1/identities/register
        // Register a new Job Seeker account
        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<ActionResult> RegisterUserAccount([FromBody] RegisterForUserCommand command)
        {
            await mediator.Send(command);
            return NoContent();
        }

        [HttpPost("register-employer")]
        [AllowAnonymous]
        public async Task<ActionResult> RegisterEmployerAccount([FromBody] RegisterForEmployerCommand command)
        {
            await mediator.Send(command);
            return NoContent();
        }

        // POST /api/v1/identities/register-admin
        // Register a new Admin account (should be restricted to authorized users only)
        [HttpPost("register-admin")]
        // [AllowAnonymous]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> RegisterAdminAccount([FromBody] RegisterForAdminCommand command)
        {
            await mediator.Send(command);
            return NoContent();
        }

        // POST /api/v1/identities/register-staff
        // Register a new Staff account (should be restricted to admin users)
        [HttpPost("register-staff")]
        [Authorize(Roles = "Admin")]
        // [AllowAnonymous]
        public async Task<ActionResult> RegisterStaffAccount([FromBody] RegisterForStaffCommand command)
        {
            await mediator.Send(command);
            return NoContent();
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<LoginResponseDTO>> Login([FromBody] LoginUserCommand command)
        {
            LoginResponseDTO response;
            try
            {
                response = await mediator.Send(command);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok(response);
        }

        // POST /api/v1/identities/send-verification-code
        // Send SMS verification code to phone number
        [HttpPost("send-verification-code")]
        [AllowAnonymous]
        public async Task<ActionResult> SendVerificationCode([FromBody] SendVerificationCodeCommand command)
        {
            try
            {
                var result = await mediator.Send(command);
                if (result)
                {
                    return Ok(new { message = "Verification code sent successfully" });
                }
                else
                {
                    return BadRequest(new { message = "Failed to send verification code" });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // POST /api/v1/identities/verify-phone
        // Verify phone number with verification code
        [HttpPost("verify-phone")]
        [AllowAnonymous]
        public async Task<ActionResult> VerifyPhone([FromBody] VerifyPhoneCommand command)
        {
            try
            {
                var result = await mediator.Send(command);
                if (result)
                {
                    return Ok(new { message = "Phone number verified successfully" });
                }
                else
                {
                    return BadRequest(new { message = "Invalid verification code" });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
