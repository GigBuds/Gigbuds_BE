namespace Gigbuds_BE.Domain.Entities.Accounts;

public class AccountExperienceTag : BaseEntity
{
    public int AccountId { get; set; }
    public int EmployerId { get; set; }
    public string JobPosition { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime? EndTime { get; set; }

    // Navigation properties
    public Account Account { get; set; }
}
