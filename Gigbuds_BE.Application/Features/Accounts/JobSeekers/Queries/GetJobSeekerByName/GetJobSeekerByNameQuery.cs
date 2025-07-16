
using Gigbuds_BE.Application.DTOs;
using Gigbuds_BE.Application.DTOs.ApplicationUsers;
using Gigbuds_BE.Application.Specifications.ApplicationUsers;
using MediatR;

namespace Gigbuds_BE.Application.Features.Accounts.JobSeekers.Queries.GetJobSeekerByName
{
    public class GetJobSeekerByNameQuery(JobSeekerGetByNameQueryParams param) : IRequest<PagedResultDto<GetJobSeekerByNameDto>>
    {
        public JobSeekerGetByNameQueryParams Param { get; } = param;
    }
}
