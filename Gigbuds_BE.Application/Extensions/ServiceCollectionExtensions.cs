using FluentValidation;
using FluentValidation.AspNetCore;
using Gigbuds_BE.Application.Commons.Helpers;
using Microsoft.AspNetCore.Http;
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

            // Add Helpers
            services.AddHttpContextAccessor();
            services.AddScoped<IUserContext, UserContext>();

            // Add AutoMapper
            services.AddAutoMapper(applicationAssembly);

            // Add Fluent Validation
            services.AddValidatorsFromAssembly(applicationAssembly)
                .AddFluentValidationAutoValidation();
        }
    }
}