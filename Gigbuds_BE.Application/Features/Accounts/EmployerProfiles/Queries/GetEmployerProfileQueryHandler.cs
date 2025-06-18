using System;
using AutoMapper;
using Gigbuds_BE.Application.DTOs.ApplicationUsers;
using Gigbuds_BE.Application.Interfaces.Repositories;
using Gigbuds_BE.Application.Interfaces.Services;
using Gigbuds_BE.Domain.Entities.Accounts;
using Gigbuds_BE.Domain.Entities.Identity;
using Gigbuds_BE.Domain.Exceptions;
using MediatR;

namespace Gigbuds_BE.Application.Features.Accounts.EmployerProfiles.Queries;

public class GetEmployerProfileQueryHandler(IUnitOfWork unitOfWork, IApplicationUserService<ApplicationUser> applicationUserService, IMapper mapper) : IRequestHandler<GetEmployerProfileQuery, EmployerProfileResponseDto>
{
    public async Task<EmployerProfileResponseDto> Handle(GetEmployerProfileQuery request, CancellationToken cancellationToken)
    {
        var employerProfile = await applicationUserService.GetByIdAsync(request.Id, ["EmployerProfile"]);
        if (employerProfile == null)
        {
            throw new NotFoundException($"Employer profile with id {request.Id} not found");
        }
        var employerProfileDto = mapper.Map<EmployerProfileResponseDto>(employerProfile.EmployerProfile);
        return employerProfileDto ?? new EmployerProfileResponseDto();
    }
}
