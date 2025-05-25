using System;
using System.Text.Json.Serialization;

namespace Gigbuds_BE.Application.DTOs.ApplicationUsers;

public class LoginResponseDTO
{
    [JsonPropertyName("id_token")]
    public string IdToken { get; set; }

    [JsonPropertyName("access_token")]
    public string AccessToken { get; set; }

    [JsonPropertyName("refresh_token")]
    public string RefreshToken { get; set; }

    [JsonPropertyName("token_type")]
    public string TokenType { get; set; } = "Bearer";
}
