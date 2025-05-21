using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gigbuds_BE.Domain.Entities.Accounts;

public class EmployerProfile : BaseEntity
{
    public int EmployerId { get; set; }
    public string CompanyEmail { get; set; }
    public string CompanyAddress { get; set; }
    public string TaxNumber { get; set; }

    public string BusinessLicense { get; set; }

    public int NumOfAvailablePost { get; set; }

    public bool IsUnlimitedPost { get; set; } = false;
    
    // Navigation property
    public virtual Account Account { get; set; }
    public virtual ICollection<BusinessApplication> BusinessApplications { get; set; }
}
