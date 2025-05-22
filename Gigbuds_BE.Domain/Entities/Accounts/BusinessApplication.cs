using System;
using Gigbuds_BE.Domain.Entities;
using Gigbuds_BE.Domain.Entities.Identity;

namespace Gigbuds_BE.Domain.Entities.Accounts;

public class BusinessApplication : BaseEntity
{
    public int EmployerId { get; set; }
    public DateTime ApplyDate { get; set; }
    public BusinessApplicationStatus ApplicationStatus { get; set; }
    
    // Navigation properties
    public virtual ApplicationUser Employer { get; set; }
}

public enum BusinessApplicationStatus
{
    Pending,
    Approved,
    Rejected
}
