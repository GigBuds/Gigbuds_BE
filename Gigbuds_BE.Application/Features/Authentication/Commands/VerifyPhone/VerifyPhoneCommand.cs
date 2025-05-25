using MediatR;

namespace Gigbuds_BE.Application.Features.Authentication.Commands.VerifyPhone;

public class VerifyPhoneCommand : IRequest<bool>
{
    public string PhoneNumber { get; set; } = string.Empty;
    public string VerificationCode { get; set; } = string.Empty;
} 