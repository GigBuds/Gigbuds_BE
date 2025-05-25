using Gigbuds_BE.Domain.Entities.Constants;
using Gigbuds_BE.Domain.Entities.Identity;

namespace Gigbuds_BE.Infrastructure.Seeders.FakeData;

public static class IdentityData
{
    public static IEnumerable<ApplicationRole> GetRoles()
    {
        List<ApplicationRole> roles = new List<ApplicationRole>
        {
            new(UserRoles.JobSeeker)
            {
                NormalizedName = UserRoles.JobSeeker.ToUpper()
            },
            new(UserRoles.Employer)
            {
                NormalizedName = UserRoles.Employer.ToUpper()
            },
            new(UserRoles.Staff)
            {
                NormalizedName = UserRoles.Staff.ToUpper()
            },
            new(UserRoles.Admin)
            {
                NormalizedName = UserRoles.Admin.ToUpper()
            }
        };

        return roles;
    }
} 