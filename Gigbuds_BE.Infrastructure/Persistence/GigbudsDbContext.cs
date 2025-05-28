using FluentEmail.Core;
using Gigbuds_BE.Domain.Entities;
using Gigbuds_BE.Domain.Entities.Accounts;
using Gigbuds_BE.Domain.Entities.Chats;
using Gigbuds_BE.Domain.Entities.Feedbacks;
using Gigbuds_BE.Domain.Entities.Identity;
using Gigbuds_BE.Domain.Entities.Jobs;
using Gigbuds_BE.Domain.Entities.Memberships;
using Gigbuds_BE.Domain.Entities.Notifications;
using Gigbuds_BE.Domain.Entities.Schedule;
using Gigbuds_BE.Domain.Entities.Transactions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Gigbuds_BE.Infrastructure.Persistence
{
    public class GigbudsDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, int>
    {
        // Account entities
        public DbSet<SkillTag> SkillTags { get; set; }
        public DbSet<EmployerProfile> EmployerProfiles { get; set; }
        public DbSet<EducationalLevel> EducationalLevels { get; set; }
        public DbSet<Follower> Followers { get; set; }
        public DbSet<AccountExperienceTag> AccountExperienceTags { get; set; }
        public DbSet<BusinessApplication> BusinessApplications { get; set; }

        // Chat entities
        public DbSet<Conversation> Conversations { get; set; }
        public DbSet<ConversationMember> ConversationMembers { get; set; }
        public DbSet<Message> Messages { get; set; }

        // Feedback entities
        public DbSet<Feedback> Feedbacks { get; set; }

        // Job entities
        public DbSet<JobPost> JobPosts { get; set; }
        public DbSet<JobApplication> JobApplications { get; set; }
        public DbSet<JobHistory> JobHistories { get; set; }
        public DbSet<JobShift> JobShifts { get; set; }
        public DbSet<JobPostSchedule> JobPostSchedules { get; set; }
        public DbSet<JobType> JobTypes { get; set; }
        public DbSet<JobPosition> JobPositions { get; set; }

        // Membership entities
        public DbSet<Membership> Memberships { get; set; }
        public DbSet<AccountMembership> AccountMemberships { get; set; }

        // Notification entities
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Template> Templates { get; set; }

        // Schedule entities
        public DbSet<JobSeekerSchedule> JobSeekerSchedules { get; set; }
        public DbSet<JobSeekerShift> JobSeekerShifts { get; set; }

        // Transaction entities
        public DbSet<TransactionRecord> TransactionRecords { get; set; }

        public GigbudsDbContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Apply all configurations in the assembly
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(GigbudsDbContext).Assembly);

            // Apply base entity to all entities
            ApplyBaseEntityToDerivedClass(modelBuilder);
        }

        /// <summary>
        /// Configuring the base entity
        /// </summary>
        /// <param name="modelBuilder"></param>
        private static void ApplyBaseEntityToDerivedClass(ModelBuilder modelBuilder)
        {
            modelBuilder.Model.GetEntityTypes()
                .Where(entityType =>
                    (typeof(BaseEntity).IsAssignableFrom(entityType.ClrType)
                    && entityType.ClrType != typeof(BaseEntity))
                ).ForEach(entityType =>
                {
                    // Configure Id
                    modelBuilder.Entity(entityType.ClrType)
                        .Property(nameof(BaseEntity.Id))
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .UseIdentityColumn();
                });
        }
    }
}
