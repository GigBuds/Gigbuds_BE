using Gigbuds_BE.Application.Features.Notifications.Commands.CreateNewNotification;
using Gigbuds_BE.Application.Interfaces.Repositories;
using Gigbuds_BE.Application.Interfaces.Services;
using Gigbuds_BE.Application.Interfaces.Services.NotificationServices;
using Gigbuds_BE.Domain.Entities.Accounts;
using Gigbuds_BE.Domain.Entities.Notifications;
using MediatR;

namespace Gigbuds_BE.Application.Features.Follows.Command;

public class FollowAUserCommandHandler(
    IUnitOfWork unitOfWork,
    ITemplatingService templatingService,
    INotificationService notificationService,
    IMediator mediator) : IRequestHandler<FollowAUserCommand, bool>
{
    public async Task<bool> Handle(FollowAUserCommand request, CancellationToken cancellationToken)
    {
        var follower = new Follower
        {
            FollowerAccountId = request.UserId,
            FollowedAccountId = request.FollowedUserId,
        };
        try
        {
            unitOfWork.Repository<Follower>().Insert(follower);
            await unitOfWork.CompleteAsync();

            var template = await templatingService.ParseTemplate(ContentType.NewFollower, new NewFollowerTemplateModel
            {
                FollowerUserName = follower.FollowerAccount.ToString(),
            });

            var notificationDto = await mediator.Send(new CreateNewNotificationCommand
            {
                UserId = follower.FollowedAccountId,
                Message = template,
                ContentType = ContentType.NewFollower,
                CreatedAt = DateTime.UtcNow,
                AdditionalPayload = new Dictionary<string, string> {
                    { "followerUserId", follower.FollowerAccountId.ToString() }
                }
            }, cancellationToken);

            await notificationService.NotifyOneEmployer(
                typeof(INotificationForEmployers).GetMethod(nameof(INotificationForEmployers.NotifyNewFollower))!,
                follower.FollowedAccountId.ToString(),
                notificationDto
            );

            return true;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
}
