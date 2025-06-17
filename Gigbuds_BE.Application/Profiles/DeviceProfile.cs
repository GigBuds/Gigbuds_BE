using AutoMapper;
using Gigbuds_BE.Application.DTOs.Notifications;
using Gigbuds_BE.Domain.Entities.Notifications;

namespace Gigbuds_BE.Application.Profiles
{
    public class DeviceProfile : Profile
    {
        public DeviceProfile()
        {
            CreateMap<DevicePushNotifications, UserDeviceDto>();
        }
    }
}
