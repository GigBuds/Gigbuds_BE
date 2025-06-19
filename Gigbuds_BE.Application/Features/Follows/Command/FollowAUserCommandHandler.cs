using Gigbuds_BE.Application.Interfaces.Repositories;
using Gigbuds_BE.Domain.Entities.Accounts;
using MediatR;

namespace Gigbuds_BE.Application.Features.Follows.Command;

public class FollowAUserCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<FollowAUserCommand, bool>
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

            // TODO: Follow user notification
            return true;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
}
