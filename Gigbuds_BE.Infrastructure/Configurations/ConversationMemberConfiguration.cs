using System;
using Gigbuds_BE.Domain.Entities.Chats;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gigbuds_BE.Infrastructure.Configurations;

internal class ConversationMemberConfiguration : IEntityTypeConfiguration<ConversationMember>
{
    public void Configure(EntityTypeBuilder<ConversationMember> builder)
    {
        // Table name
        builder.ToTable("ConversationMembers", "dbo");
        
        // Configure composite primary key
        builder.HasKey(cm => new { cm.ConversationId, cm.AccountId });

        // Properties
        builder.Property(cm => cm.ConversationId)
            .IsRequired();

        builder.Property(cm => cm.AccountId)
            .IsRequired();

        builder.Property(cm => cm.JoinedDate)
            .IsRequired();

        builder.Property(cm => cm.LeaveDate)
            .IsRequired(false);

        // Relationships with Conversation
        builder.HasOne(cm => cm.Conversation)
            .WithMany(c => c.ConversationMembers)
            .HasForeignKey(cm => cm.ConversationId)
            .OnDelete(DeleteBehavior.Cascade);
            
        // Relationships with Account
        builder.HasOne(cm => cm.Account)
            .WithMany(cm => cm.ConversationMembers)
            .HasForeignKey(cm => cm.AccountId)
            .OnDelete(DeleteBehavior.Restrict);
    }
} 