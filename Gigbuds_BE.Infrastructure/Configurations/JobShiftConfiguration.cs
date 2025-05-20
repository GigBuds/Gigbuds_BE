using System;
using Gigbuds_BE.Domain.Entities.Jobs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gigbuds_BE.Infrastructure.Configurations;

internal class JobShiftConfiguration : IEntityTypeConfiguration<JobShift>
{
    public void Configure(EntityTypeBuilder<JobShift> builder)
    {
        // Table name
        builder.ToTable("JobShifts", "dbo");
            
        // Properties
        builder.Property(js => js.JobPostScheduleId)
            .IsRequired();
            
        builder.Property(js => js.DayOfWeek)
            .IsRequired();
            
        builder.Property(js => js.StartTime)
            .IsRequired();
            
        builder.Property(js => js.EndTime)
            .IsRequired();
            
        // Relationships
        builder.HasOne(js => js.JobPostSchedule)
            .WithMany(jps => jps.JobShifts)
            .HasForeignKey(js => js.JobPostScheduleId)
            .OnDelete(DeleteBehavior.Cascade);
    }
} 