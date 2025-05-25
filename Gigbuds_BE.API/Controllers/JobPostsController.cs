using Gigbuds_BE.Application.Features.JobPosts.Commands.CreateJobPost;
using Gigbuds_BE.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Wolverine;

namespace Gigbuds_BE.API.Controllers
{
    public class JobPostsController(IMessageBus messageBus) : _BaseApiController
    {
        [HttpPost]
        public async Task<ActionResult<int>> CreateJobPost([FromBody] CreateJobPostCommand jobPostDto)
        {
            int createdJobPostId;
            try
            {
                createdJobPostId = await messageBus.InvokeAsync<int>(jobPostDto);
            } catch (CreateFailedException)
            {
                return BadRequest("Failed to create job post");
            }
            return Created(string.Empty, createdJobPostId);
        }
    }
}
