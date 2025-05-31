using System;
using Gigbuds_BE.Domain.Entities.Accounts;
using Gigbuds_BE.Domain.Entities.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gigbuds_BE.Infrastructure.Configurations;

internal class EmployerProfileConfiguration : IEntityTypeConfiguration<EmployerProfile>
{
    public void Configure(EntityTypeBuilder<EmployerProfile> builder)
    {
        // Table name
        builder.ToTable("EmployerProfiles", "public");
        
        //Ignore
        builder.Ignore(ep => ep.Id);

        // Properties
        builder.Property(ep => ep.CompanyEmail)
            .HasMaxLength(255)
            .IsRequired(false);
            
        builder.Property(ep => ep.CompanyAddress)
            .HasMaxLength(500)
            .IsRequired(false);
            
        builder.Property(ep => ep.TaxNumber)
            .HasMaxLength(20)
            .IsRequired(false);
            
        builder.Property(ep => ep.BusinessLicense)
            .IsRequired(false);
            
        builder.Property(ep => ep.NumOfAvailablePost)
            .IsRequired()
            .HasDefaultValue(0);
            
        builder.Property(ep => ep.IsUnlimitedPost)
            .IsRequired()
            .HasDefaultValue(false);
            
        builder.Property(ep => ep.CompanyName)
            .IsRequired(false);
        
        builder.Property(ep => ep.CompanyLogo)
            .IsRequired(false);

        // Relationship with ApplicationUser - One-to-One
        builder.HasOne(ep => ep.Account)
            .WithOne(a => a.EmployerProfile)
            .HasForeignKey<EmployerProfile>(ep => ep.Id)
            .OnDelete(DeleteBehavior.Cascade);
            
    }
} 