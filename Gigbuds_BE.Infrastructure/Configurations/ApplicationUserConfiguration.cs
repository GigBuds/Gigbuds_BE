using Gigbuds_BE.Application.Commons.Constants;
using Gigbuds_BE.Domain.Entities.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gigbuds_BE.Infrastructure.Configurations;

internal class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        //Table name
        builder.ToTable("AspNetUsers", "public");

        //Unique index
        builder.HasIndex(a => a.Email)
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
            .IsRequired(false);

        builder.Property(a => a.IsMale)
            .HasDefaultValue(true);

        builder.Property(a => a.AvailableJobApplication)
            .HasDefaultValue(ProjectConstant.Free_Tier_Job_Application);

        builder.Property(a => a.IsEnabled)
            .HasDefaultValue(true);

        builder.Property(a => a.CreatedAt)
            .HasDefaultValueSql("NOW()");

        builder.Property(a => a.UpdatedAt)
            .HasDefaultValueSql("NOW()");

        builder.Property(a => a.AvatarUrl)
            .IsRequired(false);    
        // Relationships with Roles handled by Identity framework
    }
} 