using Gigbuds_BE.Domain.Entities.Identity;

namespace Gigbuds_BE.Domain.Entities.Accounts;

public class SkillTag : BaseEntity
{
    public string SkillName { get; set; }
    public virtual ICollection<ApplicationUser> Accounts { get; set; }
}
