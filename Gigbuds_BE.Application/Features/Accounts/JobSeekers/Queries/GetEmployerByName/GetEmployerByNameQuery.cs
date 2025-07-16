using Gigbuds_BE.Application.DTOs;
using Gigbuds_BE.Application.DTOs.ApplicationUsers;
using Gigbuds_BE.Application.Specifications.ApplicationUsers;
using MediatR;

namespace Gigbuds_BE.Application.Features.Accounts.JobSeekers.Queries.GetEmployerByName
{
    public class GetEmployerByNameQuery(EmployerGetByNameQueryParams param) : IRequest<PagedResultDto<GetEmployerByNameDto>>
    {
        public EmployerGetByNameQueryParams Param { get; } = param;
    }
}