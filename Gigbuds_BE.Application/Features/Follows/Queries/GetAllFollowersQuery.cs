using System;
using Gigbuds_BE.Application.DTOs.Followers;
using MediatR;

namespace Gigbuds_BE.Application.Features.Follows.Queries;

public class GetAllFollowersQuery : IRequest<List<FollowerResponseDto>>
{
    public int UserId { get; private set; }

    public GetAllFollowersQuery(int userId) => UserId = userId;
}
