using Gigbuds_BE.Domain.Entities.Accounts;

namespace Gigbuds_BE.Domain.Entities.Jobs;

public class JobApplication : BaseEntity
{
    public int JobPostId { get; set; }
    public int AccountId { get; set; }
    public string CvUrl { get; set; }
    public JobApplicationStatus ApplicationStatus { get; set; }
    public DateTime AppliedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    // Navigation properties
    public virtual Account Account { get; set; }
    public virtual JobPost JobPost { get; set; }
}

public enum JobApplicationStatus
{
    Pending,
    Approved,
    Rejected,
    Removed
}

