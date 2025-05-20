using System;
using System.Collections.Generic;
using Gigbuds_BE.Domain.Entities.Accounts;
using Gigbuds_BE.Domain.Entities.Notifications;
using Gigbuds_BE.Domain.Entities.Schedule;

namespace Gigbuds_BE.Domain.Entities.Jobs;

public class JobPost : BaseEntity
{
    public int AccountId { get; set; }
    public int ScheduleId { get; set; }
    public string JobTitle { get; set; }
    public string JobDescription { get; set; }
    public string JobRequirement { get; set; }
    public string ExperienceRequirement { get; set; }
    public int Salary { get; set; }
    public SalaryUnit SalaryUnit { get; set; }
    public string JobLocation { get; set; }
    public DateTime ExpireTime { get; set; }
    public string Benefit { get; set; }
    public JobPostStatus JobPostStatus { get; set; }
    public int VacancyCount { get; set; }
    public bool IsOutstandingPost { get; set; }
    
    // Navigation properties
    public virtual Account Account { get; set; }
    public virtual ICollection<JobApplication> JobApplications { get; set; }
    public virtual ICollection<JobHistory> JobHistories { get; set; }
    public virtual ICollection<Notification> Notifications { get; set; }
    public virtual JobPostSchedule JobPostSchedule { get; set; }
}

public enum SalaryUnit
{
    Shift,
    Hour,
    Day
}

public enum JobPostStatus
{
    Closed,
    Open,
    Expired
}

