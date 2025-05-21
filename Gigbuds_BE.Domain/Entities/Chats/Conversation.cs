using System;
using System.Collections.Generic;
using Gigbuds_BE.Domain.Entities;

namespace Gigbuds_BE.Domain.Entities.Chats;

public class Conversation : BaseEntity
{
    public string ConversationName { get; set; }
    // Navigation properties
    public virtual ICollection<Message> Messages { get; set; }
    public virtual ICollection<ConversationMember> ConversationMembers { get; set; }
}
