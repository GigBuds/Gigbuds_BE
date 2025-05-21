namespace Gigbuds_BE.Domain.Entities.Accounts;

public class EducationalLevel : BaseEntity
{
    public int AccountId { get; set; }
    public EducationalLevelType Level { get; set; }
    public string Major { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string SchoolName { get; set; }

    // Navigation properties
    public Account Account { get; set; }
}
public enum EducationalLevelType
{
    Primary,
    Secondary,
    High,
    University
}