namespace Gigbuds_BE.Domain.Entities.Notifications;

public class Template : BaseEntity
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string TemplateBody { get; set; }
    public ContentType ContentType { get; set; }

    // Navigation properties
    public virtual ICollection<Notification> Notifications { get; set; }
}

public enum ContentType
{
    #region General
    Email,
    #endregion

    #region User notification (Job seeker & Employer)
    MembershipExpired,
    #endregion

    #region Employer notifications
    NewJobApplicationReceived,
    NewFeedbackReceived,
    NewFollower,
    JobPostExpired,
    #endregion

    #region Job seeker notifications
    NewPostFromFollowedEmployer,
    JobFeedbackReceived,
    JobFeedbackSent,
    JobApplicationAccepted,
    JobApplicationRejected,
    JobApplicationRemovedFromApproved,
    JobCompleted,
    NewJobPostMatching,
    ProfileViewedByEmployer,
    #endregion
}

public record NewJobPostMatchingTemplateModel()
{
    public required string JobTitle { get; set; }
    public required string JobCompany { get; set; }
    public required DateOnly JobDeadline { get; set; }
    public required string DistrictCode { get; set; }
    public required string ProvinceCode { get; set; }
}