using System;
using Gigbuds_BE.Domain.Entities.Jobs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gigbuds_BE.Infrastructure.Configurations;

internal class JobPostScheduleConfiguration : IEntityTypeConfiguration<JobPostSchedule>
{
    public void Configure(EntityTypeBuilder<JobPostSchedule> builder)
    {
        // Table name
        builder.ToTable("JobPostSchedules", "dbo");
        //Ignore
        builder.Ignore(jps => jps.Id);

        // Set JobPostId as primary key
        builder.HasKey(jps => jps.JobPostId);
        
        // Properties
        builder.Property(jps => jps.JobPostId)
            .HasColumnName("JobPostScheduleId")
            .IsRequired();
            
        builder.Property(jps => jps.ShiftCount)
            .IsRequired();
            
        builder.Property(jps => jps.MinimumShift)
            .IsRequired();
        
        // Relationships - One-to-One with JobPost
        builder.HasOne(jps => jps.JobPost)
            .WithOne(jp => jp.JobPostSchedule)
            .HasForeignKey<JobPostSchedule>(jps => jps.JobPostId)
            .OnDelete(DeleteBehavior.Cascade);
            
        // Relationship with JobShift is defined in JobShiftConfiguration
    }
} 