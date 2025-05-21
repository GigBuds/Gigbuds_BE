using System;
using Gigbuds_BE.Domain.Entities.Accounts;
using Gigbuds_BE.Domain.Entities.Feedbacks;

namespace Gigbuds_BE.Domain.Entities.Jobs;

public class JobHistory : BaseEntity
{
    public int AccountId { get; set; }
    public int JobPostId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    // Navigation properties
    public virtual Account Account { get; set; }
    public virtual JobPost JobPost { get; set; }
    public virtual ICollection<Feedback> Feedbacks { get; set; }
}
