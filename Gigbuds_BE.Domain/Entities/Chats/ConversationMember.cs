using System;
using Gigbuds_BE.Domain.Entities.Accounts;
using Gigbuds_BE.Domain.Entities.Identity;

namespace Gigbuds_BE.Domain.Entities.Chats;

public class ConversationMember : BaseEntity
{
    public int ConversationId { get; set; }
    public int AccountId { get; set; }
    public bool IsAdmin { get; set; }
    public DateTime JoinedDate { get; set; }
    public DateTime? LeaveDate { get; set; }
    
    // Navigation properties
    public virtual Conversation Conversation { get; set; }
    public virtual ApplicationUser Account { get; set; }
}
