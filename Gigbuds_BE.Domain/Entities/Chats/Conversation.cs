using System;
using System.Collections.Generic;
using Gigbuds_BE.Domain.Entities;

namespace Gigbuds_BE.Domain.Entities.Chats;

public class Conversation : BaseEntity
{
    public string? NameOne { get; set; }
    public string? NameTwo { get; set; }
    public string? AvatarOne { get; set; }
    public string? AvatarTwo { get; set; }
    public string? LastMessage { get; set; }
    public string? LastMessageSenderName { get; set; }
    public string? CreatorId { get; set; }
    // Navigation properties
    public virtual ICollection<Message> Messages { get; set; }
    public virtual ICollection<ConversationMember> ConversationMembers { get; set; }
}
