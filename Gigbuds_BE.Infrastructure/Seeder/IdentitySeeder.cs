namespace Gigbuds_BE.Infrastructure.Seeder;

using Gigbuds_BE.Application.Commons.Constants;
using Gigbuds_BE.Application.Interfaces.Utilities.Seeding;
using Gigbuds_BE.Domain.Entities.Identity;
using Gigbuds_BE.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;


public class IdentitySeeder(
    ILogger<IdentitySeeder> logger,
    GigbudsDbContext dbContext,
    UserManager<ApplicationUser> userManager) : IIdentitySeeder
{
    public async Task SeedAsync()
    {
        if (await dbContext.Database.CanConnectAsync())
            try
            {
                IEnumerable<ApplicationUser> users = null;
                if (!dbContext.Roles.Any())
                {
                    var roles = IdentityData.GetRoles();
                    dbContext.Roles.AddRange(roles);
                    await dbContext.SaveChangesAsync();
                }
                if (!dbContext.Users.Any())
                {
                    users = IdentityData.GetUsers();
                    foreach (var user in users) await userManager.CreateAsync(user, "Password1!");
                }
                if (users is not null && !dbContext.UserRoles.Any()) await GetUserRoles(users);
            }
            catch (Exception ex)
            {
                logger.LogError("{ex}", ex.Message);
            }
    }

    private async Task GetUserRoles(IEnumerable<ApplicationUser> users)
    {
        foreach (var user in users)
        {
            //student one -> student
            var strippedUserRole = user.FirstName.ToLower().Split(' ')[0];
            switch (strippedUserRole)
            {
                case "jobseeker":
                    await userManager.AddToRoleAsync(user, ProjectConstant.UserRoles.JobSeeker);
                    break;
                case "employer":
                    await userManager.AddToRoleAsync(user, ProjectConstant.UserRoles.Employer);
                    break;

                case "admin":
                    await userManager.AddToRoleAsync(user, ProjectConstant.UserRoles.Admin);
                    break;

                case "staff":
                    await userManager.AddToRoleAsync(user, ProjectConstant.UserRoles.Staff);
                    break;
            }
        }
    }
}