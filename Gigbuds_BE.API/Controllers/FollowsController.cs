using System;
using Gigbuds_BE.Application.Features.Follows;
using Gigbuds_BE.Application.Features.Follows.Command;
using Gigbuds_BE.Application.Features.Follows.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Gigbuds_BE.API.Controllers;

public class FollowsController(IMediator mediator) : _BaseApiController
{
    [HttpPost("follow")]
    public async Task<IActionResult> FollowAUser(int userId, int followedUserId)
    {
        try {
            var command = new FollowAUserCommand(userId, followedUserId);
            var result = await mediator.Send(command);
            return Ok(new { message = $"Followed user {followedUserId} successfully" });
        } catch (Exception ex) {
            return BadRequest(new { message = "Failed to follow user", error = ex.Message });
        }
    }

    [HttpDelete("unfollow")]
    public async Task<IActionResult> UnfollowAUser(int userId, int followedUserId)
    {
        try {
            var command = new UnfollowAUserCommand(userId, followedUserId);
            var result = await mediator.Send(command);
            return Ok(new { message = $"Unfollowed user {followedUserId} successfully" });
        } catch (Exception ex) {
            return BadRequest(new { message = "Failed to unfollow user", error = ex.Message });
        }
    }

    [HttpGet("all-followers/{userId}")]
    public async Task<IActionResult> GetAllFollowers(int userId)
    {
        try {
            var query = new GetAllFollowersQuery(userId);
            var result = await mediator.Send(query);
            return Ok(result);
        } catch (Exception ex) {
            return BadRequest(new { message = "Failed to get all followers", error = ex.Message });
        }
    }
}
