using Gigbuds_BE.Application.Commons.Constants;
using Gigbuds_BE.Application.Configurations;
using Gigbuds_BE.Application.Interfaces.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace Gigbuds_BE.Infrastructure.Services;

public class RedisVerificationCodeService : IVerificationCodeService
{
    private readonly IDatabase _database;
    private readonly RedisSettings _settings;
    private readonly ILogger<RedisVerificationCodeService> _logger;

    public RedisVerificationCodeService(
        IConnectionMultiplexer redis, 
        IOptions<RedisSettings> settings,
        ILogger<RedisVerificationCodeService> logger)
    {
        _database = redis.GetDatabase();
        _settings = settings.Value;
        _logger = logger;
    }

    public async Task<string> GenerateVerificationCodeAsync(string phoneNumber)
    {
        try
        {
            // Generate a 6-digit verification code
            var random = new Random();
            var verificationCode = random.Next(100000, 999999).ToString();
            
            var key = GetKey(phoneNumber);
            var expiration = TimeSpan.FromMinutes(_settings.VerificationCodeExpirationMinutes);
            
            await _database.StringSetAsync(key, verificationCode, expiration);
            
            _logger.LogInformation("Generated verification code for phone number: {PhoneNumber}", phoneNumber);
            
            return verificationCode;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating verification code for phone number: {PhoneNumber}", phoneNumber);
            throw;
        }
    }

    public async Task<bool> VerifyCodeAsync(string phoneNumber, string code)
    {
        try
        {
            var key = GetKey(phoneNumber);
            var storedCode = await _database.StringGetAsync(key);
            
            if (!storedCode.HasValue)
            {
                _logger.LogWarning("No verification code found for phone number: {PhoneNumber}", phoneNumber);
                return false;
            }
            
            var isValid = storedCode == code;
            
            if (isValid)
            {
                // Remove the code after successful verification
                await _database.KeyDeleteAsync(key);
                _logger.LogInformation("Verification code verified successfully for phone number: {PhoneNumber}", phoneNumber);
            }
            else
            {
                _logger.LogWarning("Invalid verification code provided for phone number: {PhoneNumber}", phoneNumber);
            }
            
            return isValid;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error verifying code for phone number: {PhoneNumber}", phoneNumber);
            return false;
        }
    }

    public async Task<bool> IsCodeValidAsync(string phoneNumber, string code)
    {
        try
        {
            var key = GetKey(phoneNumber);
            var storedCode = await _database.StringGetAsync(key);
            
            if (!storedCode.HasValue)
            {
                return false;
            }
            
            return storedCode == code;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking code validity for phone number: {PhoneNumber}", phoneNumber);
            return false;
        }
    }

    public async Task RemoveCodeAsync(string phoneNumber)
    {
        try
        {
            var key = GetKey(phoneNumber);
            await _database.KeyDeleteAsync(key);
            _logger.LogInformation("Removed verification code for phone number: {PhoneNumber}", phoneNumber);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error removing verification code for phone number: {PhoneNumber}", phoneNumber);
            throw;
        }
    }

    private string GetKey(string phoneNumber)
    {
        return $"{_settings.VerificationCodePrefix}{phoneNumber}";
    }
}