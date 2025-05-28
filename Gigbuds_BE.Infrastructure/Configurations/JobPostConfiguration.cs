using System;
using Gigbuds_BE.Application.Commons.Constants;
using Gigbuds_BE.Domain.Entities.Jobs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gigbuds_BE.Infrastructure.Configurations;

internal class JobPostConfiguration : IEntityTypeConfiguration<JobPost>
{
    public void Configure(EntityTypeBuilder<JobPost> builder)
    {
        // Table name
        builder.ToTable("JobPosts", "public");
        
        // Properties
        builder.Property(jp => jp.AccountId)
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
            .HasConversion(
                convertToProviderExpression: s => s.ToString(),
                convertFromProviderExpression: s => Enum.Parse<JobPostStatus>(s))
            .HasDefaultValue(JobPostStatus.Open);
            
        builder.Property(jp => jp.VacancyCount)
            .IsRequired();
            
        builder.Property(jp => jp.IsOutstandingPost)
            .IsRequired()
            .HasDefaultValue(false);
            
        builder.Property(jp => jp.IsMale)
            .IsRequired().HasDefaultValue(true);
            
        builder.Property(jp => jp.AgeRequirement)
            .IsRequired(false);
            
        builder.Property(jp => jp.DistrictCode)
            .HasMaxLength(255)
            .IsRequired();
            
        builder.Property(jp => jp.ProvinceCode)
            .HasMaxLength(255)
            .IsRequired();
            
        builder.Property(jp => jp.PriorityLevel)
            .IsRequired().HasDefaultValue(ProjectConstant.Default_Priority_Level);
        // Relationships
        builder.HasOne(jp => jp.Account)
            .WithMany(a => a.JobPosts)
            .HasForeignKey(jp => jp.AccountId)
            .OnDelete(DeleteBehavior.Restrict);
            
        builder.HasMany(jp => jp.JobApplications)
            .WithOne(ja => ja.JobPost)
            .HasForeignKey(ja => ja.JobPostId)
            .OnDelete(DeleteBehavior.Cascade);
    }
} 