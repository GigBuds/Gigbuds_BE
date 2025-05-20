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
        builder.ToTable("Conversations", "dbo");

        // Properties
        builder.Property(c => c.ConversationName)
            .HasMaxLength(200)
            .IsRequired(false);

    }
} 