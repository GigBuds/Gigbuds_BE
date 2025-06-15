
using Gigbuds_BE.Application.Interfaces.Repositories;
using Gigbuds_BE.Application.Specifications.Notifications;
using Gigbuds_BE.Domain.Entities.Notifications;
using MediatR;

namespace Gigbuds_BE.Application.Features.Notifications.Commands.MarkNotificationsAsRead
{
    internal class MarkNotificationsAsReadCommandHandler : IRequestHandler<MarkNotificationsAsReadCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        public MarkNotificationsAsReadCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task Handle(MarkNotificationsAsReadCommand request, CancellationToken cancellationToken)
        {
            List<Task> tasks = [];
            foreach (var notificationId in request.NotificationIds)
            {
                tasks.Add(MarkNotificationAsRead(notificationId));
            }
            await Task.WhenAll(tasks);
        }
        private async Task MarkNotificationAsRead(int notificationId)
        {
            var notification = await _unitOfWork.Repository<Notification>().GetBySpecificationAsync(new NotificationSpecification(notificationId));
            if (notification != null)
            {
                notification.IsRead = true;
                _unitOfWork.Repository<Notification>().Update(notification);
                await _unitOfWork.CompleteAsync();
            }
        }
    }
}
