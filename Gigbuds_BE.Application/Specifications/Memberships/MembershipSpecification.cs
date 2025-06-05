using System;
using Gigbuds_BE.Domain.Entities.Memberships;

namespace Gigbuds_BE.Application.Specifications.Memberships;

public class GetMembershipByIdSpecification : BaseSpecification<Membership>
{
    public GetMembershipByIdSpecification(int id) : base(m => m.Id == id)
    {
    }
}

public class GetAccountMembershipByAccountIdAndMembershipIdSpecification : BaseSpecification<AccountMembership>
{
    public GetAccountMembershipByAccountIdAndMembershipIdSpecification(int accountId, int membershipId) : base(am => am.AccountId == accountId && am.MembershipId == membershipId)
    {
        AddInclude(am => am.Membership);
    }
}

public class GetAccountMembershipByAccountIdAndMembershipTypeSpecification : BaseSpecification<AccountMembership>
{
    public GetAccountMembershipByAccountIdAndMembershipTypeSpecification(int accountId, MembershipType membershipType) : base(am => am.AccountId == accountId && am.Membership.MembershipType == membershipType)
    {
        AddInclude(am => am.Membership);
    }
}

// public class GetAccountMembershipByTypeAndTitleSpecification : BaseSpecification<AccountMembership>
// {
//     public GetAccountMembershipByTypeAndTitleSpecification(int accountId, int membershipId, MembershipType membershipType, string title) : base(am => am.AccountId == accountId && am.MembershipId == membershipId && am.Membership.MembershipType == membershipType && am.Membership.Title == title)
//     {
//     }
// }
