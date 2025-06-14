using Gigbuds_BE.Domain.Entities.Accounts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gigbuds_BE.Infrastructure.Configurations;

internal class SkillTagConfiguration : IEntityTypeConfiguration<SkillTag>
{
    public void Configure(EntityTypeBuilder<SkillTag> builder)
    {
        //Table name
        builder.ToTable("SkillTags", "public");

        //Properties
        builder.Property(s => s.SkillName)
            .HasMaxLength(255)
            .IsRequired();
        
        builder.HasMany(s => s.Accounts)
            .WithMany(a => a.SkillTags);
    }
}
