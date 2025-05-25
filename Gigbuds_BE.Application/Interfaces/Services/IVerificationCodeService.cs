namespace Gigbuds_BE.Application.Interfaces.Services;

public interface IVerificationCodeService
{
    Task<string> GenerateVerificationCodeAsync(string phoneNumber);
    Task<bool> VerifyCodeAsync(string phoneNumber, string code);
    Task<bool> IsCodeValidAsync(string phoneNumber, string code);
    Task RemoveCodeAsync(string phoneNumber);
} 