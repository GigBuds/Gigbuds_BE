using Gigbuds_BE.Application.Interfaces.Services;
using Gigbuds_BE.Domain.Entities.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Gigbuds_BE.Application.Features.Authentication.Commands.VerifyPhone;

public class VerifyPhoneHandler : IRequestHandler<VerifyPhoneCommand, bool>
{
    private readonly IVerificationCodeService _verificationCodeService;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ILogger<VerifyPhoneHandler> _logger;

    public VerifyPhoneHandler(
        IVerificationCodeService verificationCodeService,
        UserManager<ApplicationUser> userManager,
        ILogger<VerifyPhoneHandler> logger)
    {
        _verificationCodeService = verificationCodeService;
        _userManager = userManager;
        _logger = logger;
    }

    public async Task<bool> Handle(VerifyPhoneCommand request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Verifying phone number: {PhoneNumber}", request.PhoneNumber);
            
            // Verify the code
            var isCodeValid = await _verificationCodeService.VerifyCodeAsync(request.PhoneNumber, request.VerificationCode);
            
            if (isCodeValid)
            {
                // Find user by phone number and mark phone as confirmed
                var user = await _userManager.Users.FirstOrDefaultAsync(u => u.PhoneNumber == request.PhoneNumber);
                if (user != null)
                {
                    user.PhoneNumberConfirmed = true;
                    await _userManager.UpdateAsync(user);
                    _logger.LogInformation("Phone number verified successfully for user: {UserId}", user.Id);
                }
                
                return true;
            }
            else
            {
                _logger.LogWarning("Invalid verification code for phone number: {PhoneNumber}", request.PhoneNumber);
                return false;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error verifying phone number: {PhoneNumber}", request.PhoneNumber);
            return false;
        }
    }
} 