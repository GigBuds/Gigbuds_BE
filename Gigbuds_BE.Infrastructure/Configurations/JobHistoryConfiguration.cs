using System;
using Gigbuds_BE.Domain.Entities.Jobs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gigbuds_BE.Infrastructure.Configurations;

internal class JobHistoryConfiguration : IEntityTypeConfiguration<JobHistory>
{
    public void Configure(EntityTypeBuilder<JobHistory> builder)
    {
        // Table name
        builder.ToTable("JobHistories", "public");
        
        // Properties
        builder.Property(jh => jh.AccountId)
            .IsRequired();
            
        builder.Property(jh => jh.JobPostId)
            .IsRequired();
            
        builder.Property(jh => jh.StartDate)
            .IsRequired();
            
        builder.Property(jh => jh.EndDate)
            .IsRequired(false);

        builder.Property(jh => jh.IsJobSeekerFeedback)
            .IsRequired()
            .HasDefaultValue(false);

        builder.HasIndex(jh => new {jh.AccountId, jh.JobPostId})
            .IsUnique()
            .HasDatabaseName("IX_JobHistory_AccountId_JobPostId");
        // Relationships
        builder.HasOne(jh => jh.Account)
            .WithMany(a => a.JobHistories)
            .HasForeignKey(jh => jh.AccountId)
            .OnDelete(DeleteBehavior.Cascade);
            
        builder.HasOne(jh => jh.JobPost)
            .WithMany(jp => jp.JobHistories)
            .HasForeignKey(jh => jh.JobPostId)
            .OnDelete(DeleteBehavior.Cascade);
    }
} 