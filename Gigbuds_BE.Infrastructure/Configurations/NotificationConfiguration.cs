using System;
using Gigbuds_BE.Domain.Entities.Notifications;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gigbuds_BE.Infrastructure.Configurations;

internal class NotificationConfiguration : IEntityTypeConfiguration<Notification>
{
    public void Configure(EntityTypeBuilder<Notification> builder)
    {
        // Table name
        builder.ToTable("Notifications", "public");
        
        // Properties
        builder.Property(n => n.Message)
            .HasMaxLength(500)
            .IsRequired();
            
        builder.Property(n => n.IsRead)
            .IsRequired()
            .HasDefaultValue(false);
            
        builder.Property(n => n.TemplateId)
            .IsRequired();
            
        builder.Property(n => n.AccountId)
            .IsRequired();
            
        builder.Property(n => n.JobPostId)
            .IsRequired(false);
            
        // Relationships
        builder.HasOne(n => n.Account)
            .WithMany(a => a.Notifications)
            .HasForeignKey(n => n.AccountId)
            .OnDelete(DeleteBehavior.Cascade);
            
        builder.HasOne(n => n.JobPost)
            .WithMany(jp => jp.Notifications)
            .HasForeignKey(n => n.JobPostId)
            .OnDelete(DeleteBehavior.Cascade);
            
        builder.HasOne(n => n.Template)
            .WithMany(t => t.Notifications)
            .HasForeignKey(n => n.TemplateId)
            .OnDelete(DeleteBehavior.Cascade);
    }
} 