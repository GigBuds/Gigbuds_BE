using System;
using Gigbuds_BE.Application.DTOs.JobApplications;
using MediatR;

namespace Gigbuds_BE.Application.Features.JobApplications.Queries;

public class GetJobApplicationsByJobPostQuery : IRequest<List<JobApplicationForJobPostDto>>
{
    public int JobPostId { get; set; }
    public GetJobApplicationsByJobPostQuery(int jobPostId)
    {
        JobPostId = jobPostId;
    }
}
