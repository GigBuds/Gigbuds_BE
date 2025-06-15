using MediatR;

namespace Gigbuds_BE.Application.Features.Notifications.Commands.RegisterNotification
{
    public class RegisterPushNotificationCommand : IRequest
    {
        public required int UserId { get; set; }
        public required string DeviceId { get; set; }
        public string? DeviceToken { get; set; }
        public required string DeviceType { get; set; }
        public required string DeviceName { get; set; }
        public required string DeviceModel { get; set; }
        public required string DeviceManufacturer { get; set; }
    }
}
