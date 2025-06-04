
using AutoMapper;
using Gigbuds_BE.Application.DTOs.ApplicationUsers;
using Gigbuds_BE.Application.Interfaces.Services;
using Gigbuds_BE.Domain.Entities.Identity;
using MediatR;

namespace Gigbuds_BE.Application.Features.Accounts.JobSeekers.Queries.GetJobSeekerById
{
    internal class GetJobSeekerByIdQueryHandler : IRequestHandler<GetJobSeekerByIdQuery, JobSeekerDto>
    {
        private readonly IApplicationUserService<ApplicationUser> _applicationUserService;
        private readonly IMapper _mapper;

        public GetJobSeekerByIdQueryHandler(IApplicationUserService<ApplicationUser> applicationUserService, IMapper mapper)
        {
            _applicationUserService = applicationUserService;
            _mapper = mapper;
        }
        public async Task<JobSeekerDto> Handle(GetJobSeekerByIdQuery request, CancellationToken cancellationToken)
        {
            var jobSeeker = await _applicationUserService.GetByIdAsync(request.Id);
            return _mapper.Map<JobSeekerDto>(jobSeeker);
        }
    }
}
