using Gigbuds_BE.Domain.Entities.Accounts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gigbuds_BE.Infrastructure.Configurations;

internal class AccountExperienceTagConfiguration : IEntityTypeConfiguration<AccountExperienceTag>
{
    public void Configure(EntityTypeBuilder<AccountExperienceTag> builder)
    {
        builder.ToTable("AccountExperienceTags", "public");

        // The Id from BaseEntity is already the primary key
        builder.Property(e => e.Id)
            .HasColumnName("AccountExperienceTagId");

        builder.Property(e => e.AccountId)
            .IsRequired();

        builder.Property(e => e.EmployerId)
            .IsRequired(false);

        builder.Property(e => e.JobPosition)
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(e => e.StartTime)
            .IsRequired(false);

        builder.Property(e => e.EndTime)
            .IsRequired(false);

        // Foreign key relationship with Account
        builder.HasOne(e => e.Account)
            .WithMany()
            .HasForeignKey(e => e.AccountId)
            .OnDelete(DeleteBehavior.Cascade);

    }
}