using System;
using Gigbuds_BE.Application.DTOs.ApplicationUsers;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Gigbuds_BE.Application.Features.Accounts.EmployerProfiles.Commands;

public class UpdateEmployerProfileCommand : IRequest<EmployerProfileResponseDto>
{
    public int AccountId { get; set; }
    public string CompanyEmail { get; set; }
    public string CompanyName { get; set; }
    public string CompanyAddress { get; set; }
    public string TaxNumber { get; set; }
    public IFormFile? BusinessLicense { get; set; }
    public IFormFile? CompanyLogo {get;set;}
}
