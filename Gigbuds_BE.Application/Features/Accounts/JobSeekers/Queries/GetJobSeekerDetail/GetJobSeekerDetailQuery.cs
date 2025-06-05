using Gigbuds_BE.Application.DTOs.ApplicationUsers;
using MediatR;

namespace Gigbuds_BE.Application.Features.Accounts.JobSeekers.Queries.GetJobSeekerDetail
{
    public class GetJobSeekerDetailQuery : IRequest<JobSeekerDetailDto>
    {
        public int JobSeekerId { get; set; }
    }
} 