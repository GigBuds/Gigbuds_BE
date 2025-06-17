using Gigbuds_BE.Domain.Entities.Notifications;
using System.Linq.Expressions;

namespace Gigbuds_BE.Application.Specifications.Notifications
{
    public class GetNotificationByIdSpecification : BaseSpecification<Notification>
    {
        public GetNotificationByIdSpecification(int notificationId) : base(
            notif => notif.Id == notificationId)
        {
        }
    }

    public class GetNotificationsQuerySpecification : BaseSpecification<Notification>
    {
        public GetNotificationsQuerySpecification(GetNotificationsQueryParams queryParams) : base(
            notif => notif.AccountId == int.Parse(queryParams.UserId))
        {
            AddPaging(
                skip: queryParams.PageSize * (queryParams.PageIndex - 1),
                take: queryParams.PageSize);
            AddOrderByDesc(notif => notif.CreatedAt);
        }
    }
}
