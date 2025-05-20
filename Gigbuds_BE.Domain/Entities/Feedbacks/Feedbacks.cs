using System;
using Gigbuds_BE.Domain.Entities;
using Gigbuds_BE.Domain.Entities.Accounts;
using Gigbuds_BE.Domain.Entities.Jobs;

namespace Gigbuds_BE.Domain.Entities.Feedbacks;

public class Feedback : BaseEntity
{
    public int AccountId { get; set; }
    public int EmployerId { get; set; }
    public int JobPostId { get; set; }
    public int FeedbackType { get; set; }
    public DateTime CreatedAt { get; set; }
    public int Rating { get; set; }
    public string Comment { get; set; }
    
    // Navigation properties
    public virtual Account Account { get; set; }
    public virtual Account Employer { get; set; }
    public virtual JobPost JobPost { get; set; }
}
