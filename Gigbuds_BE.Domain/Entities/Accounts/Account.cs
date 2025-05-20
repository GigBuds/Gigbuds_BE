using Gigbuds_BE.Domain.Entities.Chats;
using Gigbuds_BE.Domain.Entities.Jobs;

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
}
