using MediatR;

namespace Gigbuds_BE.Application.Features.Authentication.Commands.SendVerificationCode;

public class SendVerificationCodeCommand : IRequest<bool>
{
    public string PhoneNumber { get; set; } = string.Empty;
} 