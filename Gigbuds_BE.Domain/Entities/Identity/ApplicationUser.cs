using Microsoft.AspNetCore.Identity;

namespace Gigbuds_BE.Domain.Entities.Identity
{
    public class ApplicationUser : IdentityUser<int> // Using int as key type
    {
    }
}
