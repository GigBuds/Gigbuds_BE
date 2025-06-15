using Gigbuds_BE.Application.DTOs.Notifications;
using MediatR;

namespace Gigbuds_BE.Application.Features.Notifications.Queries.GetDevicesOfUser
{
    public class GetDevicesOfUserQuery : IRequest<List<UserDeviceDto>>
    {
        public required int UserId { get; set; }
    }
}
