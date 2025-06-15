using MediatR;

namespace Gigbuds_BE.Application.Features.Notifications.Queries.GetDeviceToken
{
    public class GetDeviceTokenQuery : IRequest<string?>
    {
        public required string DeviceId { get; set; }
    }
}
