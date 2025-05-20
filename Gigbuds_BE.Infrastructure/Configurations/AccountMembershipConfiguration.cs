using Gigbuds_BE.Domain.Entities.Memberships;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gigbuds_BE.Infrastructure.Configurations;

internal class AccountMembershipConfiguration : IEntityTypeConfiguration<AccountMembership>
{
    public void Configure(EntityTypeBuilder<AccountMembership> builder)
    {
        builder.ToTable("AccountMemberships", "dbo");

        builder.Property(a => a.StartDate)
            .IsRequired();

        builder.Property(a => a.EndDate)
            .IsRequired();

        builder.Property(a => a.Status)
            .HasConversion(
            convertToProviderExpression: s => s.ToString(),
            convertFromProviderExpression: s => Enum.Parse<AccountMembershipStatus>(s))
            .HasDefaultValue(AccountMembershipStatus.Active);
    }
}
