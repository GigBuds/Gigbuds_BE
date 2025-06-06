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
    Email,
    NewJobPostMatching,
    FeedbackReceived,
    MembershipExpiring,
}

public record NewJobPostMatchingTemplateModel()
{
    public required string JobTitle { get; set; }
    public required string JobCompany { get; set; }
    public required DateOnly JobDeadline { get; set; }
}