
using AutoMapper;
using Gigbuds_BE.Application.Commons.Constants;
using Gigbuds_BE.Application.DTOs;
using Gigbuds_BE.Application.DTOs.ApplicationUsers;
using Gigbuds_BE.Application.Interfaces.Repositories;
using Gigbuds_BE.Application.Interfaces.Services;
using Gigbuds_BE.Application.Specifications.ApplicationUsers;
using Gigbuds_BE.Domain.Entities.Identity;
using MediatR;

namespace Gigbuds_BE.Application.Features.Accounts.JobSeekers.Queries.GetJobSeekerByName
{
    internal class GetJobSeekerByNameQueryHandler : IRequestHandler<GetJobSeekerByNameQuery, PagedResultDto<GetJobSeekerByNameDto>>
    {
        private readonly IApplicationUserService<ApplicationUser> _applicationUserService;
        private readonly IMapper _mapper;
        public GetJobSeekerByNameQueryHandler(IApplicationUserService<ApplicationUser> applicationUserService, IMapper mapper)
        {
            _applicationUserService = applicationUserService;
            _mapper = mapper;
        }

        public async Task<PagedResultDto<GetJobSeekerByNameDto>> Handle(GetJobSeekerByNameQuery request, CancellationToken cancellationToken)
        {
            var jobSeekers = await _applicationUserService.GetUsersByRoleAsync(ProjectConstant.UserRoles.JobSeeker);
            var nameQuery = request.Param.Name?.ToLower() ?? string.Empty;

            var jobSeekersWithSpec = jobSeekers
                .Where(x => x.IsEnabled &&
                            (string.IsNullOrEmpty(nameQuery) ||
                             (x.FirstName?.ToLower().Contains(nameQuery) ?? false) ||
                             (x.LastName?.ToLower().Contains(nameQuery) ?? false) ||
                             (x.UserName?.ToLower().Contains(nameQuery) ?? false)))
                .ToList();

            var mappedResult = _mapper.Map<List<GetJobSeekerByNameDto>>(jobSeekersWithSpec);
            return new PagedResultDto<GetJobSeekerByNameDto>(mappedResult.Count, mappedResult);
        }
    }
}
