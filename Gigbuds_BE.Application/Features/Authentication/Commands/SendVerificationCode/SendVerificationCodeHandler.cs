using Gigbuds_BE.Application.Interfaces.Services;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Gigbuds_BE.Application.Features.Authentication.Commands.SendVerificationCode;

public class SendVerificationCodeHandler : IRequestHandler<SendVerificationCodeCommand, bool>
{
    private readonly IVerificationCodeService _verificationCodeService;
    private readonly ISmsService _smsService;
    private readonly ILogger<SendVerificationCodeHandler> _logger;

    public SendVerificationCodeHandler(
        IVerificationCodeService verificationCodeService,
        ISmsService smsService,
        ILogger<SendVerificationCodeHandler> logger)
    {
        _verificationCodeService = verificationCodeService;
        _smsService = smsService;
        _logger = logger;
    }

    public async Task<bool> Handle(SendVerificationCodeCommand request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Generating verification code for phone number: {PhoneNumber}", request.PhoneNumber);
            
            // Generate verification code and store in Redis
            var verificationCode = await _verificationCodeService.GenerateVerificationCodeAsync(request.PhoneNumber);
            
            // Send SMS
            var smsSent = await _smsService.SendVerificationCodeAsync(request.PhoneNumber, verificationCode);
            
            if (smsSent)
            {
                _logger.LogInformation("Verification code sent successfully to phone number: {PhoneNumber}", request.PhoneNumber);
                return true;
            }
            else
            {
                _logger.LogError("Failed to send verification code to phone number: {PhoneNumber}", request.PhoneNumber);
                // Remove the code from Redis if SMS failed
                await _verificationCodeService.RemoveCodeAsync(request.PhoneNumber);
                return false;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending verification code to phone number: {PhoneNumber}", request.PhoneNumber);
            return false;
        }
    }
} 