using System;
using Gigbuds_BE.Domain.Entities.Memberships;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gigbuds_BE.Infrastructure.Configurations;

internal class AccountMembershipConfiguration : IEntityTypeConfiguration<AccountMembership>
{
    public void Configure(EntityTypeBuilder<AccountMembership> builder)
    {
        //Table name
        builder.ToTable("AccountMemberships", "dbo");

        //Ignore
        builder.Ignore(am => am.Id);
        
        //Composite key
        builder.HasKey(am => new { am.AccountId, am.MembershipId });

        //Properties
        builder.Property(am => am.AccountId)
            .IsRequired();
            
        builder.Property(am => am.MembershipId)
            .IsRequired();

        builder.Property(am => am.StartDate)
            .IsRequired();

        builder.Property(am => am.EndDate)
            .IsRequired();

        builder.Property(am => am.Status)
            .HasConversion(
            convertToProviderExpression: s => s.ToString(),
            convertFromProviderExpression: s => Enum.Parse<AccountMembershipStatus>(s))
            .HasDefaultValue(AccountMembershipStatus.Active);
            
        // Relationships
        builder.HasOne(am => am.Account)
            .WithMany(a => a.AccountMemberships)
            .HasForeignKey(am => am.AccountId)
            .OnDelete(DeleteBehavior.Cascade);
            
        builder.HasOne(am => am.Membership)
            .WithMany(m => m.AccountMemberships)
            .HasForeignKey(am => am.MembershipId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
