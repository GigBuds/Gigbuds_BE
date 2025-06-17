using System;
using AutoMapper;
using Gigbuds_BE.Application.DTOs.Followers;
using Gigbuds_BE.Application.Interfaces.Repositories;
using Gigbuds_BE.Application.Specifications.Follows;
using Gigbuds_BE.Domain.Entities.Accounts;
using MediatR;

namespace Gigbuds_BE.Application.Features.Follows.Queries;

public class GetAllFollowersQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<GetAllFollowersQuery, List<FollowerResponseDto>>
{
    public async Task<List<FollowerResponseDto>> Handle(GetAllFollowersQuery request, CancellationToken cancellationToken)
    {
        var spec = new GetAllFollowersSpec(request.UserId);
        var followers = await unitOfWork.Repository<Follower>().GetAllWithSpecificationProjectedAsync<FollowerResponseDto>(spec, mapper.ConfigurationProvider);

        return followers.ToList() ?? new List<FollowerResponseDto>();
    }
}
