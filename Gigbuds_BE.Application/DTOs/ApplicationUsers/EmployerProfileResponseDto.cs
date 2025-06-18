using System;

namespace Gigbuds_BE.Application.DTOs.ApplicationUsers;

public class EmployerProfileResponseDto
{
    public string CompanyEmail { get; set; }
    public string CompanyName { get; set; }
    public string CompanyAddress { get; set; }
    public string TaxNumber { get; set; }
    public string BusinessLicense { get; set; }
    public string CompanyLogo { get; set; }
}
