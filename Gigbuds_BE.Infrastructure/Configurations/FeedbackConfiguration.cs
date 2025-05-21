using System;
using Gigbuds_BE.Domain.Entities.Feedbacks;
using Microsoft.Build.Execution;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gigbuds_BE.Infrastructure.Configurations;

internal class FeedbackConfiguration : IEntityTypeConfiguration<Feedback>
{
    public void Configure(EntityTypeBuilder<Feedback> builder)
    {
        // Table name
        builder.ToTable("Feedbacks", "dbo");

        // Properties
        builder.Property(f => f.AccountId)
            .IsRequired();
            
        builder.Property(f => f.EmployerId)
            .IsRequired();
            
        builder.Property(f => f.JobHistoryId)
            .IsRequired();
            
        builder.Property(f => f.FeedbackType)
            .IsRequired()
            .HasConversion(
                convertToProviderExpression: s => s.ToString(),
                convertFromProviderExpression: s => Enum.Parse<FeedbackType>(s));
            
        builder.Property(f => f.CreatedAt)
            .IsRequired();
            
        builder.Property(f => f.Rating)
            .IsRequired();
            
        builder.Property(f => f.Comment)
            .HasMaxLength(1000)
            .IsRequired(false);
            
        // Relationships
        builder.HasOne(f => f.Account)
            .WithMany(a => a.Feedbacks)
            .HasForeignKey(f => f.AccountId)
            .OnDelete(DeleteBehavior.Restrict);
            
        builder.HasOne(f => f.Employer)
            .WithMany()
            .HasForeignKey(f => f.EmployerId)
            .OnDelete(DeleteBehavior.Restrict);
            
        builder.HasOne(f => f.JobHistory)
            .WithMany(j => j.Feedbacks)
            .HasForeignKey(f => f.JobHistoryId)
            .OnDelete(DeleteBehavior.Cascade);
    }
} 