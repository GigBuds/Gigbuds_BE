using System.Net;
using System.Text;
using System.Text.Json;
using Gigbuds_BE.Application.Configurations;
using Gigbuds_BE.Application.Interfaces.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Gigbuds_BE.Infrastructure.Services;

public class SpeedSmsService : ISmsService
{
    private readonly SpeedSmsSettings _settings;
    private readonly ILogger<SpeedSmsService> _logger;
    private readonly HttpClient _httpClient;
    private const string RootUrl = "https://api.speedsms.vn/index.php";

    public SpeedSmsService(IOptions<SpeedSmsSettings> settings, ILogger<SpeedSmsService> logger, HttpClient httpClient)
    {
        _settings = settings.Value;
        _logger = logger;
        _httpClient = httpClient;
        
        // Set up Basic Authentication
        var credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{_settings.AccessToken}:x"));
        _httpClient.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", credentials);
    }

    public async Task<bool> SendVerificationCodeAsync(string phoneNumber, string verificationCode)
    {
        var message = $"Mã xác thực của bạn là: {verificationCode}. Mã có hiệu lực trong 5 phút.";
        return await SendSmsAsync(phoneNumber, message);
    }

    public async Task<bool> SendSmsAsync(string phoneNumber, string message)
    {
        try
        {
            var url = $"{RootUrl}/sms/send";
            
            // Ensure phone number is in correct format
            var formattedPhone = FormatPhoneNumber(phoneNumber);
            
            var payload = new
            {
                to = new[] { formattedPhone },
                content = message,
                type = _settings.Type,
                sender = _settings.DeviceId
            };

            var jsonContent = JsonSerializer.Serialize(payload);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            _logger.LogInformation("Sending SMS to {PhoneNumber}", formattedPhone);
            
            var response = await _httpClient.PostAsync(url, content);
            var responseContent = await response.Content.ReadAsStringAsync();
            
            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation("SMS sent successfully to {PhoneNumber}. Response: {Response}", 
                    formattedPhone, responseContent);
                return true;
            }
            else
            {
                _logger.LogError("Failed to send SMS to {PhoneNumber}. Status: {StatusCode}, Response: {Response}", 
                    formattedPhone, response.StatusCode, responseContent);
                return false;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending SMS to {PhoneNumber}", phoneNumber);
            return false;
        }
    }

    private static string FormatPhoneNumber(string phoneNumber)
    {
        // Remove any spaces, dashes, or other formatting
        var cleaned = phoneNumber.Replace(" ", "").Replace("-", "").Replace("(", "").Replace(")", "");
        
        // If it starts with 0, replace with 84 (Vietnam country code)
        if (cleaned.StartsWith("0"))
        {
            cleaned = "84" + cleaned.Substring(1);
        }
        // If it doesn't start with 84, add it
        else if (!cleaned.StartsWith("84"))
        {
            cleaned = "84" + cleaned;
        }
        
        return cleaned;
    }
} 