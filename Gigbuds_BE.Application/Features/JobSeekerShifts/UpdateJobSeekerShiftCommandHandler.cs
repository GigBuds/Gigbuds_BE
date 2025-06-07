using AutoMapper;
using Gigbuds_BE.Application.DTOs.JobSeekerShifts;
using Gigbuds_BE.Application.Interfaces.Repositories;
using Gigbuds_BE.Application.Interfaces.Services;
using Gigbuds_BE.Application.Specifications.ApplicationUsers;
using Gigbuds_BE.Application.Specifications.JobShifts;
using Gigbuds_BE.Domain.Entities.Identity;
using Gigbuds_BE.Domain.Entities.Schedule;
using Gigbuds_BE.Domain.Exceptions;
using MediatR;

namespace Gigbuds_BE.Application.Features.JobSeekerShifts;

public class UpdateJobSeekerShiftCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IApplicationUserService<ApplicationUser> applicationUserService) : IRequestHandler<UpdateJobSeekerShiftCommand, IReadOnlyList<JobSeekerShiftResponseDto>>
{
    public async Task<IReadOnlyList<JobSeekerShiftResponseDto>> Handle(UpdateJobSeekerShiftCommand request, CancellationToken cancellationToken)
    {
        var userSpec = new JobSeekerByAccountIdSpecification(request.JobSeekerId);
        var updatedJobSeekerShifts = new List<JobSeekerShiftResponseDto>();
        var jobSeeker = await applicationUserService.GetUserWithSpec(userSpec);
        if (jobSeeker == null)
        {
            throw new NotFoundException("Job seeker not found");
        }
        
        jobSeeker.JobSeekerSchedule.JobShifts.Clear();
        foreach (var shift in request.JobShifts)
        {
            var toAddedJobSeekerShift = mapper.Map<JobSeekerShift>(shift);
            toAddedJobSeekerShift.JobSeekerScheduleId = request.JobSeekerId;
            jobSeeker.JobSeekerSchedule.JobShifts.Add(toAddedJobSeekerShift);
            
            updatedJobSeekerShifts.Add(mapper.Map<JobSeekerShiftResponseDto>(toAddedJobSeekerShift));
        }

        await unitOfWork.CompleteAsync();

        return updatedJobSeekerShifts;
    }
}
