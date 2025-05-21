namespace Gigbuds_BE.Domain.Entities.Accounts;

public class SkillTag : BaseEntity
{
    public string SkillName { get; set; }
    public int AccountId { get; set; }
    public virtual Account Account { get; set; }
}
