using System;

namespace Gigbuds_BE.Domain.Entities.Jobs;

public class JobPosition : BaseEntity
{
    public string JobPositionName { get; set; }
    public int JobTypeId { get; set; }
    public virtual JobType JobType { get; set; }
    public virtual ICollection<JobPost> JobPosts { get; set; }
}
