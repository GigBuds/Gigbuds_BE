using System;
using Gigbuds_BE.Domain.Entities.Accounts;

namespace Gigbuds_BE.Application.Specifications.Follows;

public class GetFollowerByUserIdAndFollowedUserIdSpec : BaseSpecification<Follower>
{
    public GetFollowerByUserIdAndFollowedUserIdSpec(int userId, int followedUserId)
        : base(f => f.FollowerAccountId == userId && f.FollowedAccountId == followedUserId)
    {
    }
}

public class GetAllFollowersSpec : BaseSpecification<Follower>
{
    public GetAllFollowersSpec(int userId)
        : base(f => f.FollowedAccountId == userId)
    {
        AddInclude(f => f.FollowerAccount);
    }
}
