using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Gigbuds_BE.Application.Configurations;
using Gigbuds_BE.Application.Interfaces.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Gigbuds_BE.Infrastructure.Services;

public class AbenlaSmsService : ISmsService
{
    private readonly AbenlaSmsSettings _settings;
    private readonly ILogger<AbenlaSmsService> _logger;
    private readonly HttpClient _httpClient;

    public AbenlaSmsService(IOptions<AbenlaSmsSettings> settings, ILogger<AbenlaSmsService> logger, HttpClient httpClient)
    {
        _settings = settings.Value;
        _logger = logger;
        _httpClient = httpClient;
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
            // Format phone number
            var formattedPhone = FormatPhoneNumber(phoneNumber);
            
            // Generate MD5 sign
            var sign = GenerateMD5Sign(_settings.SendSmsPassword);
            
            // Generate unique SMS GUID
            var smsGuid = Guid.NewGuid().ToString();

            // Build the GET request URL
            var url = $"{_settings.RootUrl}/SendSms" +
                     $"?loginName={Uri.EscapeDataString(_settings.LoginName)}" +
                     $"&sign={Uri.EscapeDataString(sign)}" +
                     $"&serviceTypeId={_settings.ServiceTypeId}" +
                     $"&phoneNumber={Uri.EscapeDataString(formattedPhone)}" +
                     $"&message={Uri.EscapeDataString(message)}" +
                     $"&brandName={Uri.EscapeDataString(_settings.BrandName)}" +
                     $"&callBack={_settings.CallBack.ToString().ToLower()}" +
                     $"&smsGuid={Uri.EscapeDataString(smsGuid)}";

            _logger.LogInformation("Sending SMS to {PhoneNumber} via Abenla API", formattedPhone);
            
            var response = await _httpClient.GetAsync(url);
            var responseContent = await response.Content.ReadAsStringAsync();
            
            if (response.IsSuccessStatusCode)
            {
                // Parse the response to check if SMS was sent successfully
                var smsResponse = JsonSerializer.Deserialize<AbenlaSmsResponse>(responseContent, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (smsResponse != null && smsResponse.Code == 106) // 106 is success code according to API docs
                {
                    _logger.LogInformation("SMS sent successfully to {PhoneNumber}. Response: {Response}", 
                        formattedPhone, responseContent);
                    return true;
                }
                else
                {
                    _logger.LogError("SMS sending failed to {PhoneNumber}. Error Code: {ErrorCode}, Message: {ErrorMessage}", 
                        formattedPhone, smsResponse?.Code, smsResponse?.Message);
                    return false;
                }
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

    private static string GenerateMD5Sign(string password)
    {
        using (var md5 = MD5.Create())
        {
            var inputBytes = Encoding.UTF8.GetBytes(password);
            var hashBytes = md5.ComputeHash(inputBytes);
            
            // Convert the byte array to hexadecimal string
            var sb = new StringBuilder();
            for (int i = 0; i < hashBytes.Length; i++)
            {
                sb.Append(hashBytes[i].ToString("x2"));
            }
            return sb.ToString();
        }
    }
}

public class AbenlaSmsResponse
{
    public int SmsPerMessage { get; set; }
    public int Code { get; set; }
    public string Message { get; set; } = string.Empty;
}