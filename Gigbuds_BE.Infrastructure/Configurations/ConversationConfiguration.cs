using System;
using Gigbuds_BE.Domain.Entities.Chats;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gigbuds_BE.Infrastructure.Configurations;

internal class ConversationConfiguration : IEntityTypeConfiguration<Conversation>
{
    public void Configure(EntityTypeBuilder<Conversation> builder)
    {
        // Table name
        builder.ToTable("Conversations", "public");
        builder.Property(c => c.NameOne)
            .HasMaxLength(255);

        builder.Property(c => c.NameTwo)
            .HasMaxLength(255);

        builder.Property(c => c.AvatarOne)
            .HasMaxLength(500);

        builder.Property(c => c.AvatarTwo)
            .HasMaxLength(500);

        builder.Property(c => c.LastMessage)
            .HasMaxLength(1000);

        builder.Property(c => c.LastMessageSenderName)
            .HasMaxLength(255);

        builder.Property(c => c.CreatorId)
            .HasMaxLength(50);

    }
} 