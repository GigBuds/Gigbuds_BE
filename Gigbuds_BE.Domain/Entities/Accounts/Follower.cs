using System;

namespace Gigbuds_BE.Domain.Entities.Accounts;

public class Follower : BaseEntity
{
    public int FollowerAccountId { get; set; }
    public int FollowedAccountId { get; set; }
    public virtual Account FollowerAccount { get; set; }
    public virtual Account FollowedAccount { get; set; }
}
