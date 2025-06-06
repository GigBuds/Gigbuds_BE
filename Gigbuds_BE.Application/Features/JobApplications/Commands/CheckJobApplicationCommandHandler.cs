using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gigbuds_BE.Application.Interfaces.Repositories;
using Gigbuds_BE.Application.Specifications.JobApplications;
using Gigbuds_BE.Domain.Entities.Jobs;
using MediatR;

namespace Gigbuds_BE.Application.Features.JobApplications.Commands
{
    public class CheckJobApplicationCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<CheckJobApplicationCommand, bool>
    {
        public async Task<bool> Handle(CheckJobApplicationCommand request, CancellationToken cancellationToken)
        {
            var jobApplication = await unitOfWork.Repository<JobApplication>().GetBySpecificationAsync(
                new GetJobSpecificationById(request.JobPostId, request.AccountId));
            return jobApplication == null;
        }
    }
}
