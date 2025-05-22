using Gigbuds_BE.Domain.Entities.Memberships;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gigbuds_BE.Infrastructure.Configurations;

internal class MembershipConfiguration : IEntityTypeConfiguration<Membership>
{
    public void Configure(EntityTypeBuilder<Membership> builder)
    {
        builder.ToTable("Memberships", "public");

        builder.Property(m => m.Title)
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(m => m.Duration)
            .IsRequired();

        builder.Property(m => m.Price)
            .IsRequired();

        builder.Property(m => m.Description)
            .IsRequired(false);

        builder.Property(m => m.MembershipType)
            .HasConversion(
            convertToProviderExpression: s => s.ToString(),
            convertFromProviderExpression: s => Enum.Parse<MembershipType>(s));
    }
}