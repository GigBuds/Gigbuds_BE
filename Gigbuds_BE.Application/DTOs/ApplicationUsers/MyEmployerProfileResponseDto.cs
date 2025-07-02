using System;

namespace Gigbuds_BE.Application.DTOs.ApplicationUsers;

public class MyEmployerProfileResponseDto
{
    public string CompanyEmail { get; set; } = string.Empty;
    public string CompanyName { get; set; } = string.Empty;
    public string CompanyAddress { get; set; } = string.Empty;
    public string TaxNumber { get; set; } = string.Empty;
    public string BusinessLicense { get; set; } = string.Empty;
    public string CompanyLogo { get; set; } = string.Empty;
    public int NumOfAvailablePost { get; set; }
    public float AverageRating { get; set; } = 0.0f;
    public int NumOfFollowers { get; set; } = 0;
    public string CompanyDescription { get; set; } = string.Empty;
}
