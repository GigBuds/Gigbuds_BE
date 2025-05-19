using FluentEmail.Core;
using Gigbuds_BE.Domain.Entities;
using Gigbuds_BE.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Gigbuds_BE.Infrastructure.Persistence
{
    public class GigbudsDbContext : IdentityDbContext<ApplicationUser, IdentityRole<int>, int>
    {
        public GigbudsDbContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Apply all configurations in the assembly
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(GigbudsDbContext).Assembly);

            // Apply base entity to all entities
            ApplyBaseEntityToDerivedClass(modelBuilder);
        }

        /// <summary>
        /// Configuring the base entity
        /// </summary>
        /// <param name="modelBuilder"></param>
        private static void ApplyBaseEntityToDerivedClass(ModelBuilder modelBuilder)
        {
            modelBuilder.Model.GetEntityTypes()
                .Where(entityType =>
                    (typeof(BaseEntity).IsAssignableFrom(entityType.ClrType)
                    && entityType.ClrType != typeof(BaseEntity))
                ).ForEach(entityType =>
                {
                    // Configure Id
                    modelBuilder.Entity(entityType.ClrType)
                        .Property(nameof(BaseEntity.Id))
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .UseIdentityColumn();
                });
        }
    }
}
