using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Gigbuds_BE.Application.Extensions
{
    public static partial class ServiceCollectionExtensions
    {
        public static void AddApplications(this IServiceCollection services, IConfiguration configuration)
        {
            // Application assembly
            var applicationAssembly = typeof(ServiceCollectionExtensions).Assembly;

            // Add AutoMapper
            services.AddAutoMapper(applicationAssembly);


            services.AddMediatR(cfg =>
            {
                // Register all handlers from the Application assembly
                cfg.RegisterServicesFromAssembly(applicationAssembly);
            });
        }
    }
}