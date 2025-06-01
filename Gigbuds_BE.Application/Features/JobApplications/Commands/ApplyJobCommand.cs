using System;
using Gigbuds_BE.Application.DTOs.JobApplicationDto;
using Gigbuds_BE.Application.DTOs.JobApplications;
using MediatR;

namespace Gigbuds_BE.Application.Features.JobApplications.Commands;

public class ApplyJobCommand : IRequest<JobApplicationResponseDto>
{
    public JobApplicationDto JobApplication { get; set; }
    
    public ApplyJobCommand(JobApplicationDto jobApplication)
    {
        JobApplication = jobApplication;
    }
}
