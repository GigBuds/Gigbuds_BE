using Gigbuds_BE.Application.Interfaces.Repositories;
using Gigbuds_BE.Application.Interfaces.Services;
using Gigbuds_BE.Infrastructure.Persistence;
using Gigbuds_BE.Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using StackExchange.Redis;

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
                    .EnableSensitiveDataLogging());

            // Add Redis
            services.AddSingleton<IConnectionMultiplexer>(config =>
            {
                ArgumentException.ThrowIfNullOrEmpty(configuration.GetConnectionString("RedisDb"));
                var connectionString = configuration.GetConnectionString("RedisDb")!;
                return ConnectionMultiplexer.Connect(connectionString);
            });

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
            services.AddScoped(typeof(IApplicationUserService<>), typeof(ApplicationUserService<>));
        }
    }
}