namespace Gigbuds_BE.Domain.Entities.Memberships;

public class AccountMembership : BaseEntity
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public AccountMembershipStatus Status { get; set; } = AccountMembershipStatus.Active;
}

public enum AccountMembershipStatus
{
    Active,
    Inactive
}
