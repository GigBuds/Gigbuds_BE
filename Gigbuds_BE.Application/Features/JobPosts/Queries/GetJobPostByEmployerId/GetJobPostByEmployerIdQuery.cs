using System;
using Gigbuds_BE.Application.DTOs;
using MediatR;

namespace Gigbuds_BE.Application.Features.JobPosts.Queries.GetJobPostByEmployerId;

public class GetJobPostByEmployerIdQuery : IRequest<List<JobPostDto>>
{
    public int EmployerId { get; set; }
    public GetJobPostByEmployerIdQuery(int employerId) => EmployerId = employerId;
}
