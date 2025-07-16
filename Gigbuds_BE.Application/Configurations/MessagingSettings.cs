namespace Gigbuds_BE.Application.Configurations
{
    public class MessagingSettings
    {
        public const string SectionName = "Messaging";

        public class StorageSettings
        {
            public const string SectionName = "Messaging:Storage";
            public string ConnectionKeyName { get; set; } = string.Empty;
            public string MessagesSectionKeyName { get; set; } = string.Empty;
            public string ConversationMetadataSectionKeyName { get; set; } = string.Empty;
            public string ConnectionString { get; set; } = string.Empty;

            public int RedisDatabase { get; set; } = 2;

        }

        public StorageSettings Storage { get; set; } = new StorageSettings();

    }
}
