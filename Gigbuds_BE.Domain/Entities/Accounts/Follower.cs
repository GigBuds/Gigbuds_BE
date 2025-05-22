using System;
using Gigbuds_BE.Domain.Entities.Identity;

namespace Gigbuds_BE.Domain.Entities.Accounts;

public class Follower : BaseEntity
{
    public int FollowerAccountId { get; set; }
    public int FollowedAccountId { get; set; }
    public virtual ApplicationUser FollowerAccount { get; set; }
    public virtual ApplicationUser FollowedAccount { get; set; }
}
