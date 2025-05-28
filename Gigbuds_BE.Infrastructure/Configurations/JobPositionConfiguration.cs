using System;
using Gigbuds_BE.Domain.Entities.Jobs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gigbuds_BE.Infrastructure.Configurations;

public class JobPositionConfiguration : IEntityTypeConfiguration<JobPosition>
{
    public void Configure(EntityTypeBuilder<JobPosition> builder)
    {
        builder.ToTable("JobPositions", "public");
        builder.HasIndex(jp => jp.JobPositionName).IsUnique();

        builder.HasOne(jp => jp.JobType)
            .WithMany(jt => jt.JobPositions)
            .HasForeignKey(jp => jp.JobTypeId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasMany(jp => jp.JobPosts)
            .WithOne(jp => jp.JobPosition)
            .HasForeignKey(jp => jp.JobPositionId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.Property(jp => jp.JobPositionName)
            .HasMaxLength(255)
            .IsRequired();
    }
}
