using System;
using Gigbuds_BE.Domain.Entities.Jobs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gigbuds_BE.Infrastructure.Configurations;

internal class JobPostConfiguration : IEntityTypeConfiguration<JobPost>
{
    public void Configure(EntityTypeBuilder<JobPost> builder)
    {
        // Table name
        builder.ToTable("JobPosts", "dbo");
        
        // Properties
        builder.Property(jp => jp.AccountId)
            .IsRequired();
            
        builder.Property(jp => jp.ScheduleId)
            .IsRequired();
            
        builder.Property(jp => jp.JobTitle)
            .HasMaxLength(50)
            .IsRequired();
            
        builder.Property(jp => jp.JobDescription)
            .IsRequired(false);
            
        builder.Property(jp => jp.JobRequirement)
            .IsRequired(false);
            
        builder.Property(jp => jp.ExperienceRequirement)
            .IsRequired(false);
            
        builder.Property(jp => jp.Salary)
            .IsRequired();
            
        builder.Property(jp => jp.SalaryUnit)
            .IsRequired()
            .HasConversion(
                convertToProviderExpression: s => s.ToString(),
                convertFromProviderExpression: s => Enum.Parse<SalaryUnit>(s));
            
        builder.Property(jp => jp.JobLocation)
            .HasMaxLength(255)
            .IsRequired(false);
            
        builder.Property(jp => jp.ExpireTime)
            .IsRequired();
            
        builder.Property(jp => jp.Benefit)
            .HasMaxLength(2000)
            .IsRequired(false);
            
        builder.Property(jp => jp.JobPostStatus)
            .IsRequired()
            .HasConversion(
                convertToProviderExpression: s => s.ToString(),
                convertFromProviderExpression: s => Enum.Parse<JobPostStatus>(s))
            .HasDefaultValue(JobPostStatus.Open);
            
        builder.Property(jp => jp.VacancyCount)
            .IsRequired();
            
        builder.Property(jp => jp.IsOutstandingPost)
            .IsRequired()
            .HasDefaultValue(false);
            
        // Relationships
        builder.HasOne(jp => jp.Account)
            .WithMany()
            .HasForeignKey(jp => jp.AccountId)
            .OnDelete(DeleteBehavior.Restrict);
            
        builder.HasMany(jp => jp.JobApplications)
            .WithOne()
            .HasForeignKey(ja => ja.JobPostId)
            .OnDelete(DeleteBehavior.Cascade);
            
    }
} 