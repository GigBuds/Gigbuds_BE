using System;
using Gigbuds_BE.Application.DTOs.ApplicationUsers;

namespace Gigbuds_BE.Application.DTOs;

public class AccountDto
{
    public DateTime Dob { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Password { get; set; }
    public string SocialSecurityNumber { get; set; }
    public bool IsMale { get; set; } = true;
    public int AvailableJobApplication { get; set; }
    public bool IsEnabled { get; set; } = true;
    public string? RefreshToken { get; set; }
    public string? CurrentLocation { get; set; }
    public string AvatarUrl { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public List<string> Roles { get; set; }
    public EmployerProfileResponseDto employerProfileResponseDto { get; set; }
}
