using System;
using System.Security.Cryptography.Pkcs;
using Gigbuds_BE.Domain.Entities.Jobs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gigbuds_BE.Infrastructure.Configurations;

internal class JobApplicationConfiguration : IEntityTypeConfiguration<JobApplication>
{
    public void Configure(EntityTypeBuilder<JobApplication> builder)
    {
        // Table name
        builder.ToTable("JobApplications", "dbo");

        //Ignore
        builder.Ignore(ja => ja.CreatedAt);

        // Properties
        builder.Property(ja => ja.JobPostId)
            .IsRequired();
            
        builder.Property(ja => ja.AccountId)
            .IsRequired();
            
        builder.Property(ja => ja.CvUrl)
            .HasMaxLength(500)
            .IsRequired(false);
            
        builder.Property(ja => ja.ApplicationStatus)
            .IsRequired();
            
        builder.Property(ja => ja.AppliedAt)
            .IsRequired();

        builder.Property(ja => ja.ApplicationStatus)
            .HasConversion(
                convertToProviderExpression: s => s.ToString(),
                convertFromProviderExpression: s => Enum.Parse<JobApplicationStatus>(s));
                
        // Relationships
        builder.HasOne(ja => ja.Account)
            .WithMany(a => a.JobApplications)
            .HasForeignKey(ja => ja.AccountId)
            .OnDelete(DeleteBehavior.Restrict);
            
        // Relationship with JobPost
        builder.HasOne(ja => ja.JobPost)
            .WithMany(jp => jp.JobApplications)
            .HasForeignKey(ja => ja.JobPostId)
            .OnDelete(DeleteBehavior.Restrict);
    }
} 