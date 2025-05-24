using Gigbuds_BE.Application.Commons.Constants;
using Gigbuds_BE.Domain.Entities.Identity;

namespace Gigbuds_BE.Infrastructure.Seeder;


internal static class IdentityData
{
    public static IEnumerable<ApplicationUser> GetUsers()
    {
        List<ApplicationUser> users = new List<ApplicationUser>
        {
            new ApplicationUser
            {
                UserName = "jobseeker1",
                SocialSecurityNumber = "123-456-789",
                NormalizedUserName = "JOBSEEKER1",
                Email = "jobseeker1@example.com",
                NormalizedEmail = "JOBSEEKER1@EXAMPLE.COM",
                FirstName = "JobSeeker",
                Password = "Password1!",
                LastName = "One",
                PhoneNumber = "123456789",
                Dob = DateTime.SpecifyKind(new DateTime(DateTime.UtcNow.Year - 25, 1, 1), DateTimeKind.Utc),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new ApplicationUser
            {
                UserName = "employer1",
                NormalizedUserName = "EMPLOYER1",
                SocialSecurityNumber = "123-456-789",
                Email = "employer1@example.com",
                NormalizedEmail = "EMPLOYER1@EXAMPLE.COM",
                Password = "Password1!",
                FirstName = "Employer",
                LastName = "One",
                PhoneNumber = "123456789",
                Dob = DateTime.SpecifyKind(new DateTime(DateTime.UtcNow.Year - 25, 1, 1), DateTimeKind.Utc),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new ApplicationUser
            {
                UserName = "admin1",
                NormalizedUserName = "ADMIN1",
                Email = "admin1@example.com",
                SocialSecurityNumber = "123-456-789",
                NormalizedEmail = "ADMIN1@EXAMPLE.COM",
                FirstName = "Admin",
                Password = "Password1!",
                LastName = "One",
                PhoneNumber = "123456789",
                Dob = DateTime.SpecifyKind(new DateTime(DateTime.UtcNow.Year - 25, 1, 1), DateTimeKind.Utc),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new ApplicationUser
            {
                UserName = "staff1",
                NormalizedUserName = "STAFF1",
                Email = "staff1@example.com",
                SocialSecurityNumber = "123-456-789",
                NormalizedEmail = "STAFF1@EXAMPLE.COM",
                FirstName = "Staff",
                LastName = "One",
                Password = "Password1!",
                PhoneNumber = "123456789",
                Dob = DateTime.SpecifyKind(new DateTime(DateTime.UtcNow.Year - 25, 1, 1), DateTimeKind.Utc),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            }
        };

        return users;
    }

    public static IEnumerable<ApplicationRole> GetRoles()
    {
        List<ApplicationRole> roles = new List<ApplicationRole>
        {
            new(ProjectConstant.UserRoles.JobSeeker)
            {
                NormalizedName = ProjectConstant.UserRoles.JobSeeker.ToUpper()
            },
            new(ProjectConstant.UserRoles.Employer)
            {
                NormalizedName = ProjectConstant.UserRoles.Employer.ToUpper()
            },
            new(ProjectConstant.UserRoles.Admin)
            {
                NormalizedName = ProjectConstant.UserRoles.Admin.ToUpper()
            },
            new(ProjectConstant.UserRoles.Staff)
            {
                NormalizedName = ProjectConstant.UserRoles.Staff.ToUpper()
            }
        };

        return roles;
    }
}
