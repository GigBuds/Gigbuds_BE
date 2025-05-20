using System;
using Gigbuds_BE.Domain.Entities.Feedbacks;
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
        builder.Property(f => f.Id)
            .HasColumnName("feedback_id");
        
        builder.Property(f => f.AccountId)
            .IsRequired();
            
        builder.Property(f => f.EmployerId)
            .IsRequired();
            
        builder.Property(f => f.JobPostId)
            .IsRequired();
            
        builder.Property(f => f.FeedbackType)
            .IsRequired()
            .HasConversion(
                convertToProviderExpression: s => (int)s,
                convertFromProviderExpression: s => (FeedbackType)s);
            
        builder.Property(f => f.CreatedAt)
            .IsRequired();
            
        builder.Property(f => f.Rating)
            .IsRequired();
            
        builder.Property(f => f.Comment)
            .HasMaxLength(1000)
            .IsRequired(false);
            
        // Relationships
        builder.HasOne(f => f.Account)
            .WithMany()
            .HasForeignKey(f => f.AccountId)
            .OnDelete(DeleteBehavior.Restrict);
            
        builder.HasOne(f => f.Employer)
            .WithMany()
            .HasForeignKey(f => f.EmployerId)
            .OnDelete(DeleteBehavior.Restrict);
            
        builder.HasOne(f => f.JobPost)
            .WithMany()
            .HasForeignKey(f => f.JobPostId)
            .OnDelete(DeleteBehavior.Cascade);
    }
} 