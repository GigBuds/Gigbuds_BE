using Gigbuds_BE.Application.Specifications;
using Gigbuds_BE.Domain.Entities.Jobs;

namespace Gigbuds_BE.Application.Specifications.JobPositions;

public class JobPositionByIdSpecification : BaseSpecification<JobPosition>
{
    public JobPositionByIdSpecification(int jobPositionId) 
        : base(jp => jp.Id == jobPositionId && jp.IsEnabled)
    {
        AddInclude(jp => jp.JobType);
    }
}

public class JobPositionByNameSpecification : BaseSpecification<JobPosition>
{
    public JobPositionByNameSpecification(string jobPositionName) 
        : base(jp => jp.JobPositionName.ToLower() == jobPositionName.ToLower() && jp.IsEnabled)
    {
    }
}

public class AllJobPositionsSpecification : BaseSpecification<JobPosition>
{
    public AllJobPositionsSpecification() 
        : base(jp => jp.IsEnabled)
    {
        AddInclude(jp => jp.JobType);
        AddOrderBy(jp => jp.JobPositionName);
    }
}

public class JobTypeByIdSpecification : BaseSpecification<JobType>
{
    public JobTypeByIdSpecification(int jobTypeId) 
        : base(jt => jt.Id == jobTypeId && jt.IsEnabled)
    {
    }
} 