using AutoMapper;
using Gigbuds_BE.Application.Commons.Constants;
using Gigbuds_BE.Application.DTOs;
using Gigbuds_BE.Application.DTOs.ApplicationUsers;
using Gigbuds_BE.Application.Interfaces.Repositories;
using Gigbuds_BE.Application.Interfaces.Services;
using Gigbuds_BE.Application.Specifications.ApplicationUsers;
using Gigbuds_BE.Domain.Entities.Identity;
using MediatR;
using System.Linq;

namespace Gigbuds_BE.Application.Features.Accounts.JobSeekers.Queries.GetEmployerByName
{
    internal class GetEmployerByNameQueryHandler : IRequestHandler<GetEmployerByNameQuery, PagedResultDto<GetEmployerByNameDto>>
    {
        private readonly IApplicationUserService<ApplicationUser> _applicationUserService;
        private readonly IMapper _mapper;
        public GetEmployerByNameQueryHandler(IApplicationUserService<ApplicationUser> applicationUserService, IMapper mapper)
        {
            _applicationUserService = applicationUserService;
            _mapper = mapper;
        }

        public async Task<PagedResultDto<GetEmployerByNameDto>> Handle(GetEmployerByNameQuery request, CancellationToken cancellationToken)
        {
            var employers = await _applicationUserService.GetUsersByRoleAsync(ProjectConstant.UserRoles.Employer);
            var nameQuery = request.Param.Name?.ToLower() ?? string.Empty;

            var employersWithSpec = employers
                .Where(x => x.IsEnabled &&
                            (string.IsNullOrEmpty(nameQuery) ||
                                (x.FirstName?.ToLower().Contains(nameQuery) ?? false) ||
                                (x.LastName?.ToLower().Contains(nameQuery) ?? false) ||
                                (x.UserName?.ToLower().Contains(nameQuery) ?? false)))
                .ToList();

            var mappedResult = _mapper.Map<List<GetEmployerByNameDto>>(employersWithSpec);
            return new PagedResultDto<GetEmployerByNameDto>(mappedResult.Count, mappedResult);
        }
    }
}