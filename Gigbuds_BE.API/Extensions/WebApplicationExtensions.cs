using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Gigbuds_BE.Domain.Entities.Constants;
using Gigbuds_BE.Domain.Entities.Identity;
using System.Threading.Tasks;

namespace Gigbuds_BE.API.Extensions;

public static class WebApplicationExtensions
{
    public static async Task SeedRolesAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var services = scope.ServiceProvider;
        var logger = services.GetRequiredService<ILogger<WebApplication>>();
        var roleManager = services.GetRequiredService<RoleManager<ApplicationRole>>();
        
        try
        {
            logger.LogInformation("Seeding roles from UserRoles.cs");
            
            // Explicitly list all roles from UserRoles.cs
            string[] roleNames = {
                UserRoles.Admin,
                UserRoles.Staff, 
                UserRoles.JobSeeker, 
                UserRoles.Employer,
                // Add any other roles defined in UserRoles.cs
            };
            
            foreach (var roleName in roleNames)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new ApplicationRole { Name = roleName });
                    logger.LogInformation("Created role: {RoleName}", roleName);
                }
            }
            
            logger.LogInformation("Role seeding completed");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while seeding roles");
            throw;
        }
    }
}
