using Gigbuds_BE.Domain.Entities.Identity;
using Gigbuds_BE.Domain.Entities.Notifications;

namespace Gigbuds_BE.Domain.Entities.Jobs;

public class JobPost : BaseEntity
{
    public int AccountId { get; set; }
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
    public bool IsMale { get; set; }
    public int? AgeRequirement { get; set; }
    public int JobPositionId { get; set; }
    public string DistrictCode { get; set; }
    public string ProvinceCode { get; set; }
    public int PriorityLevel { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }  

    // Navigation properties
    public virtual ApplicationUser Account { get; set; }
    public virtual ICollection<JobApplication> JobApplications { get; set; }
    public virtual ICollection<JobHistory> JobHistories { get; set; }
    public virtual ICollection<Notification> Notifications { get; set; }
    public virtual JobPostSchedule JobPostSchedule { get; set; }
    public virtual JobPosition JobPosition { get; set; }
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
    Expired,
    Finished
}

