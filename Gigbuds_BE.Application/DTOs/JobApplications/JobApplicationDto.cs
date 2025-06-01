using System;
using Microsoft.AspNetCore.Http;

namespace Gigbuds_BE.Application.DTOs.JobApplications;

public class JobApplicationDto
{
    public int JobPostId { get; set; }
    public int AccountId { get; set; }
    public IFormFile? CvFile { get; set; }
}

public class JobApplicationResponseDto
{
    public int Id { get; set; }
    public int JobPostId { get; set; }
    public int AccountId { get; set; }
    public string? CvUrl { get; set; }
    public string ApplicationStatus { get; set; } = string.Empty;
    public DateTime AppliedAt { get; set; }
}
