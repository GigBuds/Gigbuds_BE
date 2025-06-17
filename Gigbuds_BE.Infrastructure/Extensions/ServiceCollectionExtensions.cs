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
using Quartz;
using StackExchange.Redis;
using Gigbuds_BE.Application.Interfaces.Services.AuthenticationServices;
using Gigbuds_BE.Infrastructure.Services.AuthenticationServices;
using Gigbuds_BE.Application.Configurations;
using Gigbuds_BE.Infrastructure.Seeder;
using Gigbuds_BE.Infrastructure.Services.SignalR;
using Gigbuds_BE.Application.Interfaces.Services.NotificationServices;
using Gigbuds_BE.Infrastructure.Services.Firebase;

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

            // Configure Notification storage settings
            services.Configure<NotificationSettings>(configuration.GetSection(NotificationSettings.MainSectionName));

            // Configure Abenla SMS settings (replacing SpeedSMS)
            services.Configure<AbenlaSmsSettings>(configuration.GetSection(AbenlaSmsSettings.SectionName));
            services.Configure<PayOSSettings>(configuration.GetSection(PayOSSettings.SectionName));

            // Configure Firebase settings
            services.Configure<FirebaseSettings>(configuration.GetSection(FirebaseSettings.SectionName));

            // Configure Google Maps settings
            services.Configure<GoogleMapsSettings>(configuration.GetSection(GoogleMapsSettings.SectionName));

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
            services.AddScoped<IPaymentService, PayOSService>();
            services.AddScoped<IVerificationCodeService, RedisVerificationCodeService>();
            services.AddScoped<IFileStorageService, FirebaseStorageService>();

            // Add Firebase service
            services.AddSingleton<IFirebaseService, FirebaseService>();

            // Add Google Maps service
            services.AddHttpClient<IGoogleMapsService, GoogleMapsService>();

            // Add Vector storage and text embedding services
            services.AddScoped<IVectorStorageService, VectorStorageService>();
            services.AddScoped<ITextEmbeddingService, TextEmbeddingService>();


            // Add SignalR service
            services.AddSignalR();
            services.AddSingleton<IConnectionManager, ConnectionManager>();
            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<IPushNotificationService, PushNotificationService>();

            // Add Notification storage service
            services.AddScoped<INotificationStorageService, RedisNotificationStorageService>();

            // Add templating service
            services.AddScoped<ITemplatingService, TemplatingService>();

            // Add Job Recommendation service
            services.AddScoped<IJobRecommendationService, JobRecommendationService>();
            services.AddScoped<IMembershipsService, MembershipsServices>();
        }
    }
}