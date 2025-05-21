using System;
using Gigbuds_BE.Domain.Entities;
using System.Collections.Generic;

namespace Gigbuds_BE.Domain.Entities.Notifications;

public class Template : BaseEntity
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string TemplateBody { get; set; }
    public ContentType ContentType { get; set; }
    
    // Navigation properties
    public virtual ICollection<Notification> Notifications { get; set; }
}

public enum ContentType
{
    Email,
    Notification
}
