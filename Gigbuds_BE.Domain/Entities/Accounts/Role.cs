using System;

namespace Gigbuds_BE.Domain.Entities.Accounts;

public class Role : BaseEntity
{
    public string RoleName { get; set; }
    public virtual ICollection<Account> Accounts { get; set; }
}
