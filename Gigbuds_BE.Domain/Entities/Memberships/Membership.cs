using Gigbuds_BE.Domain.Entities.Transactions;

namespace Gigbuds_BE.Domain.Entities.Memberships;

public class Membership : BaseEntity
{
    public string Title { get; set; }
    public MembershipType MembershipType { get; set; }
    public int Duration { get; set; }
    public decimal Price { get; set; }
    public string Description { get; set; }
    
    // Navigation properties
    public virtual ICollection<TransactionRecord> Transactions { get; set; }
    public virtual ICollection<AccountMembership> AccountMemberships { get; set; }
}

public enum MembershipType
{
    JobSeeker,
    Employer
}
