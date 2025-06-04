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
    }
}

public class GetAccountMembershipForRevokeSpecification : BaseSpecification<AccountMembership>
{
    public GetAccountMembershipForRevokeSpecification(int accountId, int membershipId, MembershipType membershipType) : base(am => am.AccountId == accountId && am.MembershipId == membershipId && am.Membership.MembershipType == membershipType)
    {
    }
}
