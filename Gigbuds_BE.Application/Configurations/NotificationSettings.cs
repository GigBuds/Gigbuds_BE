
using System.ComponentModel.DataAnnotations;

namespace Gigbuds_BE.Application.Configurations
{
    public class NotificationSettings
    {
        public const string MainSectionName = "Notification";
        public string DistanceThresholdInKilometers { get; set; } = "10";

        public class StorageSettings
        {
            public const string SectionName = "Notification:Storage";

            [Required]
            public string ConnectionString { get; set; } = string.Empty;

            public int RedisDatabase { get; set; } = 1;

            public string RedisKeyPrefix { get; set; } = "Gigbuds:Notifications:User";
        }

        public StorageSettings Storage { get; set; } = new StorageSettings();
    }
}
