using Gigbuds_BE.Domain.Entities.Identity;

namespace Gigbuds_BE.Domain.Entities.Jobs;

public class JobApplication : BaseEntity
{
    public int JobPostId { get; set; }
    public int AccountId { get; set; }
    public string CvUrl { get; set; }
    public JobApplicationStatus ApplicationStatus { get; set; }

    // Navigation properties
    public virtual ApplicationUser Account { get; set; }
    public virtual JobPost JobPost { get; set; }
}

public enum JobApplicationStatus
{
    Pending,
    Approved,
    Rejected,
    Removed
}

