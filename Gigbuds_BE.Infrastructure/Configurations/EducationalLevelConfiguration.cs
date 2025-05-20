using Gigbuds_BE.Domain.Entities.Accounts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gigbuds_BE.Infrastructure.Configurations;

internal class EducationalLevelConfiguration : IEntityTypeConfiguration<EducationalLevel>
{
    public void Configure(EntityTypeBuilder<EducationalLevel> builder)
    {
        //Table name
        builder.ToTable("EducationalLevels", "dbo");

        // Properties
        builder.Property(e => e.AccountId)
            .IsRequired();

        builder.Property(e => e.Level)
            .HasConversion(
            convertToProviderExpression: s => s.ToString(),
            convertFromProviderExpression: s => Enum.Parse<EducationalLevelType>(s))
            .IsRequired(true);

        builder.Property(e => e.Major)
            .HasMaxLength(255)
            .IsRequired(false);

        builder.Property(e => e.StartDate)
            .IsRequired(false);

        builder.Property(e => e.EndDate)
            .IsRequired(false);

        builder.Property(e => e.SchoolName)
            .HasMaxLength(255)
            .IsRequired(false);


        // Foreign key relationship with Account
        builder.HasOne(e => e.Account)
            .WithMany()
            .HasForeignKey(e => e.AccountId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}