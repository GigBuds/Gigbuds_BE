using Gigbuds_BE.Domain.Entities.Accounts;
using Gigbuds_BE.Domain.Entities.Identity;
using Gigbuds_BE.Domain.Entities.Memberships;

namespace Gigbuds_BE.Domain.Entities.Transactions;

public class TransactionRecord : BaseEntity
{
    public decimal Revenue { get; set; }
    public TransactionStatus TransactionStatus { get; set; }
    public string Content { get; set; }
    public string Gateway { get; set; }
    public string ReferenceCode { get; set; }
    public int? MembershipId { get; set; }
    public int AccountId { get; set; }

    // Navigation properties
    public virtual Membership Membership { get; set; }
    public virtual ApplicationUser Account { get; set; }
}

public enum TransactionStatus
{
    Pending,
    Completed,
    Failed,
    Refunded,
    Cancelled
}
