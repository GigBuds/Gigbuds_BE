using System;
using Gigbuds_BE.Domain.Entities.Chats;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gigbuds_BE.Infrastructure.Configurations;

internal class MessageConfiguration : IEntityTypeConfiguration<Message>
{
    public void Configure(EntityTypeBuilder<Message> builder)
    {
        // Table name
        builder.ToTable("Messages", "dbo");

        // Properties
        builder.Property(m => m.Content)
            .HasMaxLength(1000)
            .IsRequired();

        builder.Property(m => m.SendDate)
            .IsRequired();

        builder.Property(m => m.ConversationId)
            .IsRequired();

        builder.Property(m => m.AccountId)
            .IsRequired();

        // Relationships
        builder.HasOne(m => m.Conversation)
            .WithMany(c => c.Messages)
            .HasForeignKey(m => m.ConversationId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(m => m.Account)
            .WithMany(a => a.Messages)
            .HasForeignKey(m => m.AccountId)
            .OnDelete(DeleteBehavior.Restrict);
    }
} 