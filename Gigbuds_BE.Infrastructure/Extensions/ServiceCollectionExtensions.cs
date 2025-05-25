using Gigbuds_BE.Application.Interfaces.Repositories;
using Gigbuds_BE.Application.Interfaces.Services;
using Gigbuds_BE.Application.Interfaces.Utilities.Seeding;
using Gigbuds_BE.Domain.Entities.Identity;
using Gigbuds_BE.Infrastructure.Persistence;
using Gigbuds_BE.Infrastructure.Seeders;
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

namespace Gigbuds_BE.Infrastructure.Extensions
{
    public static partial class ServiceCollectionExtensions
    {
        public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            //Add swagger
            services.AddSwaggerGen(option =>
            {
                //JWT Config
                option.DescribeAllParametersInCamelCase();
                option.ResolveConflictingActions(conf => conf.First());     // duplicate API name if any, ex: Get() & Get(string id)
                option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter a valid token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });
                option.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type=ReferenceType.SecurityScheme,
                                Id="Bearer"
                            }
                        },
                        new string[]{}
                    }
                });
            });

            // Add sql server
            services.AddDbContextPool<GigbudsDbContext>(options =>
                options
                    .UseNpgsql(configuration.GetConnectionString("GigbudsDb"))
                    .EnableSensitiveDataLogging()
                    .EnableDetailedErrors());

            // Add Identity services with role support (but without authentication middleware)
            services.AddIdentityCore<ApplicationUser>(options =>
            {
                // Configure Identity options if needed
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
            })
            .AddRoles<ApplicationRole>()
            .AddEntityFrameworkStores<GigbudsDbContext>()
            .AddDefaultTokenProviders()
            .AddSignInManager<SignInManager<ApplicationUser>>();

            // Add Redis
            services.AddSingleton<IConnectionMultiplexer>(config =>
            {
                ArgumentException.ThrowIfNullOrEmpty(configuration.GetConnectionString("RedisDb"));
                var connectionString = configuration.GetConnectionString("RedisDb")!;
                return ConnectionMultiplexer.Connect(connectionString);
            });

            // Configure Redis settings
            services.Configure<RedisSettings>(configuration.GetSection(RedisSettings.SectionName));
            
            // Configure SpeedSMS settings
            services.Configure<SpeedSmsSettings>(configuration.GetSection(SpeedSmsSettings.SectionName));

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

            // Add seed identity
            services.AddScoped<IIdentitySeeder, IdentitySeeder>();

            // Add UOW
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Add ApplicationUser service
            services.AddScoped(typeof(UserManager<>));
            services.AddScoped(typeof(RoleManager<>));
            services.AddScoped(typeof(IApplicationUserService<>), typeof(ApplicationUserService<>));
            services.AddScoped<IUserTokenService, UserTokenService>();
            
            // Add SMS and Verification services
            services.AddHttpClient<ISmsService, SpeedSmsService>();
            services.AddScoped<IVerificationCodeService, RedisVerificationCodeService>();
            
            // Add Seeders
            services.AddScoped<IIdentitySeeder, IdentitySeeder>();
        }
    }
}