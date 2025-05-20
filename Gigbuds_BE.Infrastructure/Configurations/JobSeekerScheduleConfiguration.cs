using Gigbuds_BE.Domain.Entities.Schedule;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gigbuds_BE.Infrastructure.Configurations;

internal class JobSeekerScheduleConfiguration : IEntityTypeConfiguration<JobSeekerSchedule>
{
    public void Configure(EntityTypeBuilder<JobSeekerSchedule> builder)
    {
        builder.ToTable("JobSeekerSchedules", "dbo");
    }
}
