
using Gigbuds_BE.Domain.Entities.Identity;

namespace Gigbuds_BE.Domain.Entities.Notifications
{
    public class DevicePushNotifications : BaseEntity
    {
        public int AccountId { get; set; }
        public string DeviceToken { get; set; }
        public string DeviceType { get; set; }
        public string DeviceId { get; set; }
        public string DeviceName { get; set; }
        public string DeviceModel { get; set; }
        public string DeviceManufacturer { get; set; }
        public ApplicationUser Account { get; set; }
    }
}