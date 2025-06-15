
using Gigbuds_BE.Domain.Entities.Notifications;
using System.Linq.Expressions;

namespace Gigbuds_BE.Application.Specifications.Notifications
{
    public class GetDeviceTokenSpecification(string deviceId) : BaseSpecification<DevicePushNotifications>(
        device => device.DeviceId.Equals(deviceId))
    {
    }

    public class GetDevicesByUserSpecification(int userId) : BaseSpecification<DevicePushNotifications>(
        device => device.AccountId == userId)
    {
    }
}
