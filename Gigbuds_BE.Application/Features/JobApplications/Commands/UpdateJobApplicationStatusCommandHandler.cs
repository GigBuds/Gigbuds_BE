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
        var jobApplication = await unitOfWork.Repository<JobApplication>().GetBySpecificationAsync(spec);
        if (jobApplication == null)
        {
            return false;
        }
        jobApplication.ApplicationStatus = request.Status;
        jobApplication.UpdatedAt = DateTime.UtcNow;
        unitOfWork.Repository<JobApplication>().Update(jobApplication);
        await unitOfWork.CompleteAsync();

        // TODO: add notification accept, remove, rejected
        return true;
    }
}
