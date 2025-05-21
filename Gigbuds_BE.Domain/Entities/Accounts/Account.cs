using Gigbuds_BE.Domain.Entities.Chats;
using Gigbuds_BE.Domain.Entities.Feedbacks;
using Gigbuds_BE.Domain.Entities.Jobs;
using Gigbuds_BE.Domain.Entities.Memberships;
using Gigbuds_BE.Domain.Entities.Notifications;
using Gigbuds_BE.Domain.Entities.Schedule;
using Gigbuds_BE.Domain.Entities.Transactions;

namespace Gigbuds_BE.Domain.Entities.Accounts;

public class Account : BaseEntity
{
    public DateTime Dob { get; set; }
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string SocialSecurityNumber { get; set; }
    public bool IsMale { get; set; } = true;
    public int AvailableJobApplication { get; set; }
    public virtual ICollection<ConversationMember> ConversationMembers { get; set; }
    public virtual ICollection<Message> Messages { get; set; }
    public virtual ICollection<Role> Roles { get; set; }
    public virtual ICollection<Follower> Followers { get; set; }
    public virtual ICollection<Follower> Following { get; set; }
    public virtual ICollection<JobApplication> JobApplications { get; set; }
    public virtual ICollection<JobHistory> JobHistories { get; set; }
    public virtual ICollection<Notification> Notifications { get; set; }
    public virtual ICollection<TransactionRecord> TransactionRecords { get; set; }
    public virtual ICollection<Feedback> Feedbacks { get; set; }
    public virtual ICollection<AccountMembership> AccountMemberships { get; set; }
    public virtual JobSeekerSchedule JobSeekerSchedule { get; set; }
    public virtual ICollection<JobPost> JobPosts { get; set; }
    public virtual ICollection<EducationalLevel> EducationalLevels { get; set; }
    public virtual ICollection<SkillTag> SkillTags { get; set; }

    // One-to-one navigation property
    public virtual EmployerProfile EmployerProfile { get; set; }
    public virtual ICollection<BusinessApplication> BusinessApplications { get; set; }
}
