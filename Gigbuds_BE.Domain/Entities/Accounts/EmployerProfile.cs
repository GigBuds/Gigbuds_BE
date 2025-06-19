using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Gigbuds_BE.Domain.Entities.Identity;

namespace Gigbuds_BE.Domain.Entities.Accounts;

public class EmployerProfile : BaseEntity
{
    public string CompanyEmail { get; set; }
    public string CompanyName { get; set; }
    public string CompanyAddress { get; set; }
    public string TaxNumber { get; set; }
    public string BusinessLicense { get; set; }
    public string CompanyLogo { get; set; }
    public int NumOfAvailablePost { get; set; }
    public bool IsUnlimitedPost { get; set; } = false;
    
    // Navigation property
    public virtual ApplicationUser Account { get; set; }
}
