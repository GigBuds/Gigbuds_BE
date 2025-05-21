using Gigbuds_BE.Domain.Entities.Schedule;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gigbuds_BE.Infrastructure.Configurations;

internal class JobSeekerScheduleConfiguration : IEntityTypeConfiguration<JobSeekerSchedule>
{
    public void Configure(EntityTypeBuilder<JobSeekerSchedule> builder)
    {
        //Table name
        builder.ToTable("JobSeekerSchedules", "dbo");

        //Ignore
        builder.Ignore(js => js.Id);

        //Properties
        builder.HasKey(js => js.AccountId);

        //Relationships
        builder.HasOne(js => js.Account)
            .WithOne(a => a.JobSeekerSchedule)
            .HasForeignKey<JobSeekerSchedule>(js => js.AccountId);
    }
}
