using Gigbuds_BE.Application.Features.JobPosts.Commands.CreateJobPost;
using Gigbuds_BE.Domain.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Gigbuds_BE.API.Controllers
{
    public class JobPostsController(IMediator mediator) : _BaseApiController
    {
        [HttpPost]
        public async Task<ActionResult<int>> CreateJobPost([FromBody] CreateJobPostCommand jobPostDto)
        {
            int createdJobPostId;
            try
            {
                createdJobPostId = await mediator.Send(jobPostDto);
            }
            catch (CreateFailedException)
            {
                return BadRequest("Failed to create job post");
            }
            return Created(string.Empty, createdJobPostId);
        }
    }
}
