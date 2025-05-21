using System;
using Gigbuds_BE.Domain.Entities.Accounts;

namespace Gigbuds_BE.Domain.Entities.Chats;

public class ConversationMember
{
    public int ConversationId { get; set; }
    public int AccountId { get; set; }
    public DateTime JoinedDate { get; set; }
    public DateTime? LeaveDate { get; set; }
    
    // Navigation properties
    public virtual Conversation Conversation { get; set; }
    public virtual Account Account { get; set; }
}
