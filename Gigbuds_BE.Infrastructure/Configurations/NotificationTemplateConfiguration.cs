using System;
using Gigbuds_BE.Domain.Entities.Notifications;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gigbuds_BE.Infrastructure.Configurations;

internal class NotificationTemplateConfiguration : IEntityTypeConfiguration<NotificationTemplate>
{
    public void Configure(EntityTypeBuilder<NotificationTemplate> builder)
    {
        // Table name
        builder.ToTable("NotificationTemplates", "dbo");
        
        // Properties
        builder.Property(nt => nt.Name)
            .HasMaxLength(255)
            .IsRequired();
            
        builder.Property(nt => nt.Description)
            .HasMaxLength(500)
            .IsRequired(false);
            
        builder.Property(nt => nt.TemplateBody)
            .HasMaxLength(2000)
            .IsRequired();
            
        builder.Property(nt => nt.ContentType)
            .IsRequired()
            .HasConversion(
                convertToProviderExpression: s => s.ToString(),
                convertFromProviderExpression: s => Enum.Parse<ContentType>(s));
            
        builder.Property(nt => nt.IsEnabled)
            .IsRequired()
            .HasDefaultValue(true);
    }
} 