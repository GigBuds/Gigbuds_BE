using System;
using Gigbuds_BE.Domain.Entities.Accounts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gigbuds_BE.Infrastructure.Configurations;

internal class FollowerConfiguration : IEntityTypeConfiguration<Follower>
{
    public void Configure(EntityTypeBuilder<Follower> builder)
    {
        //Table name
        builder.ToTable("Followers", "public");

        //Composite key
        builder.HasKey(f => new { f.FollowerAccountId, f.FollowedAccountId });

        //Ignore
        builder.Ignore(f => f.Id);
        
        //Relationships
        builder.HasOne(f => f.FollowerAccount)
            .WithMany(a => a.Following)
            .HasForeignKey(f => f.FollowerAccountId);

        builder.HasOne(f => f.FollowedAccount)
            .WithMany(a => a.Followers)
            .HasForeignKey(f => f.FollowedAccountId);
    }
}
