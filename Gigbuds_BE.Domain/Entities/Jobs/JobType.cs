using System;

namespace Gigbuds_BE.Domain.Entities.Jobs;

public class JobType : BaseEntity
{
    public string JobTypeName { get; set; }
    public virtual ICollection<JobPosition> JobPositions { get; set; }
}
