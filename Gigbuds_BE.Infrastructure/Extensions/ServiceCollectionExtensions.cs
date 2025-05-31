using Gigbuds_BE.Application.Interfaces.Repositories;
using Gigbuds_BE.Application.Interfaces.Services;
using Gigbuds_BE.Application.Interfaces.Utilities.Seeding;
using Gigbuds_BE.Domain.Entities.Identity;
using Gigbuds_BE.Infrastructure.Persistence;
using Gigbuds_BE.Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Quartz;
using StackExchange.Redis;
using Gigbuds_BE.Application.Interfaces.Services.AuthenticationServices;
using Gigbuds_BE.Infrastructure.Services.AuthenticationServices;
using Gigbuds_BE.Application.Configurations;
using Gigbuds_BE.Infrastructure.Seeder;
using Gigbuds_BE.Application.Interfaces;
using Gigbuds_BE.Infrastructure.Repositories;

namespace Gigbuds_BE.Infrastructure.Extensions
{
    public static partial class ServiceCollectionExtensions
    {
        public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            

            // Add sql server
            services.AddDbContextPool<GigbudsDbContext>(options =>
                options
                    .UseNpgsql(configuration.GetConnectionString("GigbudsDb"))
                    .EnableSensitiveDataLogging()
                    .EnableDetailedErrors());

            services.AddIdentityApiEndpoints<ApplicationUser>()
            .AddRoles<ApplicationRole>()
            .AddEntityFrameworkStores<GigbudsDbContext>();

            // Add Redis
            services.AddSingleton<IConnectionMultiplexer>(config =>
            {
                ArgumentException.ThrowIfNullOrEmpty(configuration.GetSection("Redis:ConnectionString").Value);
                var connectionString = configuration.GetSection("Redis:ConnectionString").Value!;
                return ConnectionMultiplexer.Connect(connectionString);
            });

            // Configure Redis settings
            services.Configure<RedisSettings>(configuration.GetSection(RedisSettings.SectionName));
            
            // Configure Abenla SMS settings (replacing SpeedSMS)
            services.Configure<AbenlaSmsSettings>(configuration.GetSection(AbenlaSmsSettings.SectionName));
            
            // Configure Firebase settings
            services.Configure<FirebaseSettings>(configuration.GetSection(FirebaseSettings.SectionName));

            // Add Quartz
            services.AddQuartz(q =>
            {
                q.UseSimpleTypeLoader();
                q.UseDefaultThreadPool(tp =>
                {
                    tp.MaxConcurrency = 10;
                });

                q.UsePersistentStore(store =>
                {
                    store.UseProperties = true;
                    store.UsePostgres(postgres =>
                    {
                        ArgumentException.ThrowIfNullOrEmpty(configuration.GetConnectionString("GigbudsDb"));
                        postgres.ConnectionString = configuration.GetConnectionString("GigbudsDb")!;
                        postgres.TablePrefix = "public.qrtz_";
                    });
                    store.UseSystemTextJsonSerializer();
                });
            });
            services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);


            // Add UOW
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Add ApplicationUser service
            services.AddScoped(typeof(UserManager<>));
            services.AddScoped(typeof(RoleManager<>));
            services.AddScoped<IIdentitySeeder, IdentitySeeder>();
            services.AddScoped(typeof(IApplicationUserService<>), typeof(ApplicationUserService<>));
            services.AddScoped<IUserTokenService, UserTokenService>();
            
            // Add SMS and File Storage services
            services.AddHttpClient<ISmsService, AbenlaSmsService>();
            services.AddScoped<IVerificationCodeService, RedisVerificationCodeService>();
            services.AddScoped<IFileStorageService, FirebaseStorageService>();
            
        }
    }
}