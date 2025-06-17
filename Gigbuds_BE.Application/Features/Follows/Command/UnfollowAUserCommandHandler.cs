using System;
using Gigbuds_BE.Application.Interfaces.Repositories;
using Gigbuds_BE.Application.Specifications.Follows;
using Gigbuds_BE.Domain.Entities.Accounts;
using MediatR;

namespace Gigbuds_BE.Application.Features.Follows.Command;

public class UnfollowAUserCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<UnfollowAUserCommand, bool>
{
    public async Task<bool> Handle(UnfollowAUserCommand request, CancellationToken cancellationToken)
    {
        var spec = new GetFollowerByUserIdAndFollowedUserIdSpec(request.UserId, request.FollowedUserId);
        var follower = await unitOfWork.Repository<Follower>().GetBySpecificationAsync(spec);
        
        if (follower == null) {
            throw new Exception("Follower not found");
        }

        try {
            unitOfWork.Repository<Follower>().Delete(follower);
            await unitOfWork.CompleteAsync();
            return true;
        } catch (Exception ex) {
            throw new Exception(ex.Message);
        }
    }
}
