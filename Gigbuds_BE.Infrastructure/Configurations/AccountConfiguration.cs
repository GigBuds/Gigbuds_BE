using Gigbuds_BE.Application.Commons.Constants;
using Gigbuds_BE.Domain.Entities.Accounts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gigbuds_BE.Infrastructure.Configurations;

internal class AccountConfiguration : IEntityTypeConfiguration<Account>
{
    public void Configure(EntityTypeBuilder<Account> builder)
    {
        //Table name
        builder.ToTable("Accounts", "dbo");

        //Unique index
        builder.HasIndex(a => a.Email)
            .IsUnique();
        builder.HasIndex(a => a.SocialSecurityNumber)
            .IsUnique();

        //Properties
        builder.Property(a => a.Email)
            .HasMaxLength(200)
            .IsRequired(false);

        builder.Property(a => a.FirstName)
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(a => a.LastName)
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(a => a.SocialSecurityNumber)
            .HasMaxLength(255)
            .IsRequired();


        builder.Property(a => a.IsMale)
            .HasDefaultValue(true);

        builder.Property(a => a.AvailableJobApplication)
            .HasDefaultValue(ProjectConstant.Free_Tier_Job_Application);

        // Relationships with Roles
        builder.HasMany(a => a.Roles)
            .WithMany(r => r.Accounts);
    }
}
