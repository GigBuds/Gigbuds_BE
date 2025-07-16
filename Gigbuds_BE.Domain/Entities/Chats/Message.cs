using System;
using Gigbuds_BE.Domain.Entities;
using Gigbuds_BE.Domain.Entities.Identity;

namespace Gigbuds_BE.Domain.Entities.Chats;

public class Message : BaseEntity
{
    public string Content { get; set; }
    public DateTime SendDate { get; set; }
    public int ConversationId { get; set; }
    public int AccountId { get; set; }
    // Navigation properties
    public virtual Conversation Conversation { get; set; }
    public virtual ApplicationUser Account { get; set; }
}