using Gigbuds_BE.Application.Interfaces.Utilities.Seeding;
using Gigbuds_BE.Domain.Entities.Identity;
using Gigbuds_BE.Infrastructure.Persistence;
using Gigbuds_BE.Infrastructure.Seeders.FakeData;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Gigbuds_BE.Infrastructure.Seeders;

public class IdentitySeeder(
    ILogger<IdentitySeeder> logger,
    GigbudsDbContext dbContext,
    UserManager<ApplicationUser> userManager,
    RoleManager<ApplicationRole> roleManager) : IIdentitySeeder
{
    public async Task SeedAsync()
    {
        if (await dbContext.Database.CanConnectAsync())
        {
            try
            {
                // Seed roles if they don't exist
                if (!dbContext.Roles.Any())
                {
                    var roles = IdentityData.GetRoles();
                    foreach (var role in roles)
                    {
                        if (!await roleManager.RoleExistsAsync(role.Name!))
                        {
                            await roleManager.CreateAsync(role);
                        }
                    }
                    await dbContext.SaveChangesAsync();
                    logger.LogInformation("Roles seeded successfully");
                }
                else
                {
                    logger.LogInformation("Roles already exist, skipping seeding");
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while seeding roles: {Message}", ex.Message);
                throw;
            }
        }
        else
        {
            logger.LogError("Cannot connect to database for seeding");
        }
    }
} 