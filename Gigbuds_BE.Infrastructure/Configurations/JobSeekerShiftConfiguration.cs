using Gigbuds_BE.Domain.Entities.Schedule;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gigbuds_BE.Infrastructure.Configurations;

internal class JobSeekerShiftConfiguration : IEntityTypeConfiguration<JobSeekerShift>
{
    public void Configure(EntityTypeBuilder<JobSeekerShift> builder)
    {
        //Table name
        builder.ToTable("JobSeekerShifts", "public");

        //Properties
        builder.Property(j => j.DayOfWeek)
            .IsRequired();

        builder.Property(j => j.StartTime)
            .IsRequired();

        builder.Property(j => j.EndTime)
            .IsRequired();

        //Relationships
        builder.HasOne(j => j.JobSeekerSchedule)
            .WithMany(j => j.JobShifts)
            .HasForeignKey(j => j.JobSeekerScheduleId);
    }
}
