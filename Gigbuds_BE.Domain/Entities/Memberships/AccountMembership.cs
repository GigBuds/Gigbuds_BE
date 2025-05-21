using Gigbuds_BE.Domain.Entities.Accounts;

namespace Gigbuds_BE.Domain.Entities.Memberships;

public class AccountMembership : BaseEntity
{
    public int AccountId { get; set; }
    public int MembershipId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public AccountMembershipStatus Status { get; set; } = AccountMembershipStatus.Active;
    
    // Navigation properties
    public virtual Account Account { get; set; }
    public virtual Membership Membership { get; set; }
}

public enum AccountMembershipStatus
{
    Active,
    Inactive
}
