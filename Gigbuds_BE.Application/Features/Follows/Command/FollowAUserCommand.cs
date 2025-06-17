using MediatR;

namespace Gigbuds_BE.Application.Features.Follows.Command;

public class FollowAUserCommand : IRequest<bool>
{
    public int UserId { get; private set; }
    public int FollowedUserId { get; private set; }

    public FollowAUserCommand(int userId, int followedUserId)
    {
        UserId = userId;
        FollowedUserId = followedUserId;
    }
}
