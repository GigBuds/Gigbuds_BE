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
public record JobApplicationAcceptedTemplateModel()
{
    public required string JobName { get; set; }
}

public record JobApplicationRejectedTemplateModel()
{
    public required string JobName { get; set; }
}

public record JobApplicationRemovedTemplateModel()
{
    public required string JobName { get; set; }
}

public record NewFollowerTemplateModel()
{
    public required string FollowerUserName { get; set; } // user id of the job seeker who followed the employer
}

public record NewFeedbackReceivedTemplateModel()
{
    public required int UserId { get; set; } // user id of the employer who received the feedback
    public required int JobSeekerId { get; set; } // user id of the job seeker who sent the feedback
    public required string FeedbackName { get; set; }
    public required string FeedbackContent { get; set; }
}

public record MembershipExpiredTemplateModel()
{
    public required string MembershipName { get; set; }
}

public record NewJobApplicationReceivedTemplateModel()
{
    public required string JobName { get; set; }
}

public record NewPostFromFollowedEmployerTemplateModel()
{
    public required string EmployerUserName { get; set; }
    public required string JobName { get; set; }
}