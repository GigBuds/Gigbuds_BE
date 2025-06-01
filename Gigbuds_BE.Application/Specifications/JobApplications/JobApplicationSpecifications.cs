using System;
using Gigbuds_BE.Domain.Entities.Jobs;

namespace Gigbuds_BE.Application.Specifications.JobApplications;

public class GetJobSpecificationById : BaseSpecification<JobApplication>
{
    public GetJobSpecificationById(int id)
        : base(x => x.Id == id)
    {
    }

    public GetJobSpecificationById(int JobPostId, int AccountId)
        : base(x => x.JobPostId == JobPostId && x.AccountId == AccountId)
    {
    }
}
