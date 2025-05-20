using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gigbuds_BE.Domain.Entities.Accounts;

public class EmployerProfile : BaseEntity
{

    [MaxLength(255)]
    public string CompanyEmail { get; set; }

    [MaxLength(500)]
    public string CompanyAddress { get; set; }

    [MaxLength(20)]
    public string TaxNumber { get; set; }

    [Column(TypeName = "text")]
    public string BusinessLicense { get; set; }

    public int NumOfAvailablePost { get; set; }

    public bool IsUnlimitedPost { get; set; } = false;
}
