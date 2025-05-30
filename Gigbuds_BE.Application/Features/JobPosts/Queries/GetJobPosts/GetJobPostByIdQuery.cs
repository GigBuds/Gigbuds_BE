using System;
using Gigbuds_BE.Application.DTOs.ApplicationUsers;
using MediatR;

namespace Gigbuds_BE.Application.Features.JobPosts.Queries.GetJobPosts;

public class GetJobPostByIdQuery : IRequest<JobPostDto>
{
    public int Id { get; set; }
    public GetJobPostByIdQuery(int id)
    {
        Id = id;
    }
}
