using System.Threading.Tasks;
using Gigbuds_BE.Application.Features.Notifications.Commands.MarkNotificationsAsRead;
using Gigbuds_BE.Application.Features.Notifications.Commands.RegisterNotification;
using Gigbuds_BE.Application.Features.Notifications.Queries.GetDeviceToken;
using Gigbuds_BE.Application.Features.Notifications.Queries.GetNotifications;
using Gigbuds_BE.Application.Features.Notifications.Queries.GetStoredNotifications;
using Gigbuds_BE.Application.Specifications.Notifications;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Gigbuds_BE.API.Controllers
{
    public class NotificationsController : _BaseApiController
    {
        private IMediator _mediator { get; }

        public NotificationsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("push-notifications/register")]
        public async Task<IActionResult> RegisterPushNotification([FromBody] RegisterPushNotificationCommand request)
        {
            await _mediator.Send(request);
            return Ok("Device registered successfully");
        }

        [HttpGet("push-notifications/device-token/{deviceId}")]
        public async Task<IActionResult> GetDeviceToken([FromRoute] string deviceId)
        {
            var deviceToken = await _mediator.Send(new GetDeviceTokenQuery { DeviceId = deviceId });
            if (deviceToken == null)
            {
                return NotFound("Device token not found");
            }
            return Ok(new { DeviceToken = deviceToken });
        }

        [HttpGet]
        public async Task<IActionResult> GetNotifications([FromQuery] GetNotificationsQueryParams queryParams)
        {
            var notifications = await _mediator.Send(new GetNotificationsQuery { Params = queryParams });
            return Ok(notifications);
        }

        [HttpGet("stored/{deviceId}")]
        public async Task<IActionResult> GetStoredNotifications([FromRoute] string deviceId)
        {
            var notifications = await _mediator.Send(new GetStoredNotificationQuery { DeviceId = deviceId });
            return Ok(notifications);
        }

        [HttpPost("read")]
        public async Task<IActionResult> MarkNotificationsAsRead([FromBody] MarkNotificationsAsReadCommand request)
        {
            await _mediator.Send(request);
            return Ok("Notifications marked as read");
        }
    }
}
