using System;
using Microsoft.AspNetCore.Http;

namespace Gigbuds_BE.Application.DTOs.JobApplicationDto;

public class JobApplicationDto
{
    public int JobPostId { get; set; }
    public int AccountId { get; set; }
    public IFormFile? CvFile { get; set; }
}

