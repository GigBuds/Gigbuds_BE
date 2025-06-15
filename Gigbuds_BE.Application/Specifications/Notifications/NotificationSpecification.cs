using Gigbuds_BE.Domain.Entities.Notifications;
using System.Linq.Expressions;

namespace Gigbuds_BE.Application.Specifications.Notifications
{
    internal class NotificationSpecification : BaseSpecification<Notification>
    {
        public NotificationSpecification(int notificationId) : base(
            notif => notif.Id == notificationId)
        {
        }
    }
}
