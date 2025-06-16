using System;
using Gigbuds_BE.Domain.Entities.Notifications;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gigbuds_BE.Infrastructure.Configurations;

internal class DevicePushNotificationConfiguration
    {
        public void Configure(EntityTypeBuilder<DevicePushNotifications> builder)
        {
            builder.ToTable("DevicePushNotifications");
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).ValueGeneratedOnAdd();

            builder.Property(e => e.AccountId).IsRequired();
            builder.Property(e => e.DeviceToken).IsRequired();
            builder.Property(e => e.DeviceId).IsRequired();
            builder.Property(e => e.DeviceType).IsRequired(false);
            builder.Property(e => e.DeviceName).IsRequired(false);
            builder.Property(e => e.DeviceModel).IsRequired(false);
            builder.Property(e => e.DeviceManufacturer).IsRequired(false);

            builder.HasOne(e => e.Account)
                .WithMany(a => a.DevicePushNotifications)
                .HasForeignKey(e => e.AccountId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }