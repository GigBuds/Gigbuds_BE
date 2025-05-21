using System;
using Gigbuds_BE.Domain.Entities.Accounts;
using Gigbuds_BE.Domain.Entities.Jobs;

namespace Gigbuds_BE.Domain.Entities.Notifications;

public class Notification : BaseEntity
{   
    public string Message { get; set; }
    public bool IsRead { get; set; }
    public int TemplateId { get; set; }
    public int AccountId { get; set; }
    public int? JobPostId { get; set; }
    
    // Navigation properties
    public virtual Account Account { get; set; }
    public virtual JobPost JobPost { get; set; }
    public virtual Template Template { get; set; }
}
