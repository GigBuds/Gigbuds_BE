
namespace Gigbuds_BE.Application.DTOs.Notifications
{
    public class UserDeviceDto
    {
        public required string DeviceId { get; set; }
        public required string DeviceType { get; set; }
        public required string DeviceToken { get; set; }
        public required string DeviceName { get; set; }
        public required string DeviceModel { get; set; }
        public required string DeviceManufacturer { get; set; }
    }
}
