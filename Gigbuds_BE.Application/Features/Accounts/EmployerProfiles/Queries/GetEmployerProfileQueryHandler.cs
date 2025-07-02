using System;
using AutoMapper;
using Gigbuds_BE.Application.DTOs.ApplicationUsers;
using Gigbuds_BE.Application.Interfaces.Repositories;
using Gigbuds_BE.Application.Interfaces.Services;
using Gigbuds_BE.Application.Specifications.ApplicationUsers;
using Gigbuds_BE.Application.Specifications.JobPosts;
using Gigbuds_BE.Domain.Entities.Accounts;
using Gigbuds_BE.Domain.Entities.Identity;
using Gigbuds_BE.Domain.Entities.Jobs;
using Gigbuds_BE.Domain.Exceptions;
using MediatR;

namespace Gigbuds_BE.Application.Features.Accounts.EmployerProfiles.Queries;

public class GetEmployerProfileQueryHandler(IUnitOfWork unitOfWork, IApplicationUserService<ApplicationUser> applicationUserService, IMapper mapper) : IRequestHandler<GetEmployerProfileQuery, MyEmployerProfileResponseDto>
{
    public async Task<MyEmployerProfileResponseDto> Handle(GetEmployerProfileQuery request, CancellationToken cancellationToken)
    {
        var spec = new GetEmployerProfileByIdSpecification(request.Id);
        var NumOfAvailablePost = await unitOfWork.Repository<JobPost>().CountAsync(new GetNumOfAvailablePostByEmployerIdSpecification(request.Id));
        var employerProfile = await applicationUserService.GetUserWithSpec(spec);
        if (employerProfile == null)
        {
            throw new NotFoundException($"Employer profile with id {request.Id} not found");
        }
        
        var employerProfileDto = mapper.Map<MyEmployerProfileResponseDto>(employerProfile);
        employerProfileDto.NumOfAvailablePost = NumOfAvailablePost;
        return employerProfileDto ?? new MyEmployerProfileResponseDto();
    }
}
