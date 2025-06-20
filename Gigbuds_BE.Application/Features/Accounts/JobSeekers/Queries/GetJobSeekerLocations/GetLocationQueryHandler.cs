using System;
using Gigbuds_BE.Application.Interfaces.Repositories;
using Gigbuds_BE.Application.Interfaces.Services;
using Gigbuds_BE.Domain.Entities.Identity;
using Gigbuds_BE.Domain.Exceptions;
using MediatR;

namespace Gigbuds_BE.Application.Features.Accounts.JobSeekers.Queries.GetJobSeekerLocations;

public class GetLocationQueryHandler(IApplicationUserService<ApplicationUser> applicationUserService) : IRequestHandler<GetLocationQuery, string>
{
    public async Task<string> Handle(GetLocationQuery request, CancellationToken cancellationToken)
    {
        var jobSeeker = await applicationUserService.GetByIdAsync(request.JobSeekerId);
        if(jobSeeker == null)
        {
            throw new NotFoundException("Job seeker not found");
        }
        return jobSeeker.CurrentLocation ?? "";
    }
}
