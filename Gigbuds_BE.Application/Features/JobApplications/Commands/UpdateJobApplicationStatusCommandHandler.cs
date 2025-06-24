using System;
using Gigbuds_BE.Application.Interfaces.Repositories;
using Gigbuds_BE.Application.Specifications.JobApplications;
using Gigbuds_BE.Domain.Entities.Jobs;
using MediatR;

namespace Gigbuds_BE.Application.Features.JobApplications.Commands;

public class UpdateJobApplicationStatusCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<UpdateJobApplicationStatusCommand, bool>
{
    public async Task<bool> Handle(UpdateJobApplicationStatusCommand request, CancellationToken cancellationToken)
    {
        var spec = new GetJobSpecificationById(request.JobApplicationId);
        var jobApplication = await unitOfWork.Repository<JobApplication>().GetBySpecificationAsync(spec,false);
        if (jobApplication == null)
        {
            return false;
        }
        jobApplication.ApplicationStatus = request.Status;
        if(request.Status == JobApplicationStatus.Approved)
        {
            jobApplication.JobPost.VacancyCount--;
        }
        if(request.Status == JobApplicationStatus.Removed)
        {
            jobApplication.JobPost.VacancyCount++;
            unitOfWork.Repository<JobHistory>().Insert(new JobHistory{
                JobPostId = jobApplication.JobPostId,
                StartDate = jobApplication.UpdatedAt,
                EndDate = DateTime.UtcNow,
                AccountId = jobApplication.AccountId,
            });
        }
        jobApplication.UpdatedAt = DateTime.UtcNow;
        await unitOfWork.CompleteAsync();

        // TODO: add notification accept, remove, rejected
        return true;
    }
}
