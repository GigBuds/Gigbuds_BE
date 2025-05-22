using System;
using Gigbuds_BE.Domain.Entities.Accounts;
using Gigbuds_BE.Domain.Entities.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gigbuds_BE.Infrastructure.Configurations;

internal class BusinessApplicationConfiguration : IEntityTypeConfiguration<BusinessApplication>
{
    public void Configure(EntityTypeBuilder<BusinessApplication> builder)
    {
        // Table name
        builder.ToTable("BusinessApplications", "public");
        
        // Properties
        builder.Property(ba => ba.Id)
            .IsRequired();
            
        builder.Property(ba => ba.EmployerId)
            .IsRequired();
            
        builder.Property(ba => ba.CreatedAt)
            .HasColumnName("AppliedAt")
            .IsRequired();
            
        builder.Property(ba => ba.ApplicationStatus)
            .IsRequired()
            .HasConversion(
                convertToProviderExpression: s => s.ToString(),
                convertFromProviderExpression: s => Enum.Parse<BusinessApplicationStatus>(s))
            .HasDefaultValue(BusinessApplicationStatus.Pending);
            
        // Relationships
        builder.HasOne(ba => ba.Employer)
            .WithMany(a => a.BusinessApplications)
            .HasForeignKey(ba => ba.EmployerId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
