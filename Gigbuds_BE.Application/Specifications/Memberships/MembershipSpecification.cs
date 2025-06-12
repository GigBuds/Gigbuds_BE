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

public class GetAllMembershipSpecification : BaseSpecification<Membership>
{
    public GetAllMembershipSpecification() : base(m => m.IsEnabled == true)
    {
        AddOrderBy(m => m.Price);
    }
}

public class GetAllMembershipByAccountIdSpecification : BaseSpecification<AccountMembership>
{
    public GetAllMembershipByAccountIdSpecification(int accountId) : base(am => am.AccountId == accountId && am.Status == AccountMembershipStatus.Active)
    {
        AddInclude(am => am.Membership);
    }
}