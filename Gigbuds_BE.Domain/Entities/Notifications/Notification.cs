using System;
using System.Runtime.Serialization;
using Gigbuds_BE.Domain.Entities.Identity;
using Gigbuds_BE.Domain.Entities.Jobs;

namespace Gigbuds_BE.Domain.Entities.Notifications;

public class Notification : BaseEntity
{
    public string Message { get; set; }
    public bool IsRead { get; set; }
    public int TemplateId { get; set; }
    public int AccountId { get; set; }
    public int? JobPostId { get; set; }

    // Navigation properties
    public virtual ApplicationUser Account { get; set; }
    public virtual JobPost JobPost { get; set; }
    public virtual Template Template { get; set; }
}
public enum NotificationType
{
    [EnumMember(Value = "job")]
    job,
    [EnumMember(Value = "feedback")]
    feedback,
    [EnumMember(Value = "schedule")]
    schedule,
    [EnumMember(Value = "profile")]
    profile,
    [EnumMember(Value = "application")]
    application,
    [EnumMember(Value = "follower")]
    follower,
    [EnumMember(Value = "membership")]
    membership
}