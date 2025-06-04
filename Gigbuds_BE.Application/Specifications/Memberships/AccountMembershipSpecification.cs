using Gigbuds_BE.Domain.Entities.Memberships;

namespace Gigbuds_BE.Application.Specifications.Memberships
{
    public class AccountMembershipSpecification : BaseSpecification<AccountMembership>
    {
        public AccountMembershipSpecification(int userId) : base(m => m.AccountId == userId)
        {
            AddInclude(m => m.Membership);
        }
    }
}
