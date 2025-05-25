namespace Gigbuds_BE.Application.Interfaces.Services;

public interface ISmsService
{
    Task<bool> SendVerificationCodeAsync(string phoneNumber, string verificationCode);
    Task<bool> SendSmsAsync(string phoneNumber, string message);
} 