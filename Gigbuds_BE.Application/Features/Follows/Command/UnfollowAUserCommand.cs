using MediatR;

namespace Gigbuds_BE.Application.Features.Follows.Command;

public class UnfollowAUserCommand : IRequest<bool>
{
    public int UserId { get; private set; }
    public int FollowedUserId { get; private set; }

    public UnfollowAUserCommand(int userId, int followedUserId)
        => (UserId, FollowedUserId) = (userId, followedUserId);
}
