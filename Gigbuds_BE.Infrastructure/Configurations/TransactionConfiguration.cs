using Gigbuds_BE.Domain.Entities.Transactions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gigbuds_BE.Infrastructure.Configurations;

internal class TransactionConfiguration : IEntityTypeConfiguration<TransactionRecord>
{
    public void Configure(EntityTypeBuilder<TransactionRecord> builder)
    {
        // Table name
        builder.ToTable("TransactionRecords", "dbo");

        // Properties
        builder.Property(t => t.Revenue)
            .IsRequired();

        builder.Property(t => t.TransactionStatus)
            .IsRequired()
            .HasConversion(
                convertToProviderExpression: s => s.ToString(),
                convertFromProviderExpression: s => Enum.Parse<TransactionStatus>(s));

        builder.Property(t => t.Content)
            .HasMaxLength(500)
            .IsRequired(false);

        builder.Property(t => t.Gateway)
            .HasMaxLength(50)
            .IsRequired(false);

        builder.Property(t => t.ReferenceCode)
            .HasMaxLength(100)
            .IsRequired(false);

        builder.Property(t => t.MembershipId)
            .IsRequired(false);

        builder.Property(t => t.AccountId)
            .IsRequired();

        // Relationships
        builder.HasOne(t => t.Membership)
            .WithMany(m => m.Transactions)
            .HasForeignKey(t => t.MembershipId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(t => t.Account)
            .WithMany(a => a.TransactionRecords)
            .HasForeignKey(t => t.AccountId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}