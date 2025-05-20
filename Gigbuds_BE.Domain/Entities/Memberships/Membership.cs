namespace Gigbuds_BE.Domain.Entities.Memberships;

public class Membership : BaseEntity
{
    public string Title { get; set; }
    public MembershipType MembershipType { get; set; }
    public int Duration { get; set; }
    public decimal Price { get; set; }
    public string Description { get; set; }
}

public enum MembershipType
{
    JobSeeker,
    Employer
}
