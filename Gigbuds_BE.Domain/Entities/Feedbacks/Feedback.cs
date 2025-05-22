using System;
using Gigbuds_BE.Domain.Entities.Identity;
using Gigbuds_BE.Domain.Entities.Jobs;

namespace Gigbuds_BE.Domain.Entities.Feedbacks;

public class Feedback : BaseEntity
{
    public int AccountId { get; set; }
    public int EmployerId { get; set; }
    public int JobHistoryId { get; set; }
    public FeedbackType FeedbackType { get; set; }
    public int Rating { get; set; }
    public string Comment { get; set; }
    
    // Navigation properties
    public virtual ApplicationUser Account { get; set; }
    public virtual ApplicationUser Employer { get; set; }
    public virtual JobHistory JobHistory { get; set; }
}

public enum FeedbackType
{
    EmployerToJobSeeker,
    JobSeekerToEmployer
} 