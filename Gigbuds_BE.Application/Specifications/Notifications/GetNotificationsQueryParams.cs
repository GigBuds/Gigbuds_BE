namespace Gigbuds_BE.Application.Specifications.Notifications
{
    public class GetNotificationsQueryParams : BasePagingParams
    {
        public required string UserId { get; set; }
    }
}
