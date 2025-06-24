using System;
using Gigbuds_BE.Domain.Entities.Jobs;

namespace Gigbuds_BE.Application.Specifications.JobHistories;

public class GetJobHistoryByIdSpecification : BaseSpecification<JobHistory>
{
    public GetJobHistoryByIdSpecification(int id) : base(x => x.Id == id)
    {
        AddInclude(x => x.JobPost);
        AddInclude(x => x.JobPost.JobApplications);
    }
}
