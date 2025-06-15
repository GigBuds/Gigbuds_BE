using System.Collections.ObjectModel;
using Gigbuds_BE.Application.DTOs;
using Gigbuds_BE.Application.DTOs.Notifications;
using Gigbuds_BE.Application.Specifications.Notifications;
using MediatR;

namespace Gigbuds_BE.Application.Features.Notifications.Queries.GetNotifications
{
    public class GetNotificationsQuery : IRequest<PagedResultDto<NotificationDto>>
    {
        public required GetNotificationsQueryParams Params { get; set; }
    }
}
