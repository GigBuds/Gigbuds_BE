using Gigbuds_BE.Domain.Entities.Accounts;
using Gigbuds_BE.Domain.Entities.Chats;
using Gigbuds_BE.Domain.Entities.Feedbacks;
using Gigbuds_BE.Domain.Entities.Jobs;
using Gigbuds_BE.Domain.Entities.Memberships;
using Gigbuds_BE.Domain.Entities.Notifications;
using Gigbuds_BE.Domain.Entities.Schedule;
using Gigbuds_BE.Domain.Entities.Transactions;
using Microsoft.AspNetCore.Identity;

namespace Gigbuds_BE.Domain.Entities.Identity
{
    public class ApplicationUser : IdentityUser<int> // Using int as key type
    {
        public DateTime Dob { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public string SocialSecurityNumber { get; set; }
        public bool IsMale { get; set; } = true;
        public int AvailableJobApplication { get; set; }
        public bool IsEnabled { get; set; } = true;
        public string? RefreshToken { get; set; }
        public string AvatarUrl { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public virtual ICollection<ConversationMember> ConversationMembers { get; set; }
        public virtual ICollection<Message> Messages { get; set; }
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
        public virtual ICollection<AccountExperienceTag> AccountExperienceTags { get; set; }

        // One-to-one navigation property
        public virtual EmployerProfile EmployerProfile { get; set; }
        public virtual ICollection<BusinessApplication> BusinessApplications { get; set; }
    }
}
