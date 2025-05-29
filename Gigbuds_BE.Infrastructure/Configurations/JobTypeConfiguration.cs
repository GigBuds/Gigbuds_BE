using System;
using Gigbuds_BE.Domain.Entities.Jobs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gigbuds_BE.Infrastructure.Configurations;

public class JobTypeConfiguration : IEntityTypeConfiguration<JobType>
{
    public void Configure(EntityTypeBuilder<JobType> builder)
    {
        builder.ToTable("JobTypes", "public");
        builder.HasIndex(jt => jt.JobTypeName).IsUnique();

    }
}
