using Gigbuds_BE.Application.Features.JobPosts.Commands.CreateJobPost;
using Gigbuds_BE.Application.Features.JobPosts.Commands.UpdateJobPost;
using Gigbuds_BE.Domain.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Gigbuds_BE.Application.Features.JobPosts.Queries;
using Gigbuds_BE.Application.DTOs;
using Gigbuds_BE.Application.DTOs.ApplicationUsers;
using Gigbuds_BE.Application.Features.JobPosts.Commands.RemoveJobPost;
using Gigbuds_BE.Application.Features.JobPosts.Commands.UpdateJobPostStatus;
using Gigbuds_BE.Domain.Entities.Jobs;
using MediatR;
using Gigbuds_BE.Application.Features.JobPosts.Queries.GetSearchJobPosts;
using Gigbuds_BE.Application.DTOs.JobPosts;
using Gigbuds_BE.API.Helpers.RequestHelpers;
using Gigbuds_BE.Application.Specifications.JobPosts;

namespace Gigbuds_BE.API.Controllers
{
    public class JobPostsController(IMediator mediator) : _BaseApiController
    {
        /// <summary>
        /// Creates a new job post.
        /// </summary>
        /// <param name="command">The command to create a job post.</param>
        /// <returns>The id of the created job post.</returns>
        [HttpPost]
        public async Task<ActionResult<int>> CreateJobPost([FromBody] CreateJobPostCommand command)
        {
            if (command == null)
                return BadRequest("Request body is required");
            int createdJobPostId;
            try
            {
                createdJobPostId = await mediator.Send(command);
            }
            catch (CreateFailedException)
            {
                return BadRequest("Failed to create job post");
            }
            return Created(string.Empty, createdJobPostId);
        }
        /// <summary>
        /// Retrieves a paged list of job posts.
        /// </summary>
        /// <param name="pageIndex">Page index (starts at 1)</param>
        /// <param name="pageSize">Page size (max 20)</param>
        /// <param name="searchTerm">Optional search term</param>
        /// <returns>PagedResultDto of JobPostDto</returns>
        [HttpGet]
        [ProducesResponseType(typeof(PagedResultDto<JobPostDto>), 200)]
        public async Task<ActionResult<PagedResultDto<JobPostDto>>> GetAllJobPosts([FromQuery] GetAllJobPostsQueryParams queryParams)
        {
            var result = await mediator.Send(new GetAllJobPostsQuery(queryParams));
            return ResultWithPagination(
                result.Data,
                result.Count,
                queryParams.PageIndex,
                queryParams.PageSize);
        }

        /// <summary>
        /// Updates the status of a job post.
        /// </summary>
        /// <param name="id">Job post id</param>
        /// <param name="status">New status (Closed, Open, Expired)</param>
        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateJobPostStatus(int id, [FromBody] UpdateJobPostStatusCommand command)
        {
            if (command == null)
                return BadRequest("Request body is required");
            command.JobPostId = id;
            try
            {
                await mediator.Send(command);
            }
            catch (NotFoundException)
            {
                return NotFound($"Job post with id {id} not found");
            }
            catch (UpdateFailedException)
            {
                return BadRequest($"Failed to update job post status with id {id}");
            }
            return NoContent();
        }

        /// <summary>
        /// Updates a job post by id.
        /// </summary>
        /// <param name="id">Job post id</param>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateJobPost(int id, [FromBody] UpdateJobPostCommand command)
        {
            if (command == null)
                return BadRequest("Request body is required");
            command.JobPostId = id;
            try
            {
                await mediator.Send(command);
            }
            catch (NotFoundException)
            {
                return NotFound($"Job post with id {id} not found");
            }
            catch (UpdateFailedException)
            {
                return StatusCode(500, $"Failed to update job post with id {id}");
            }
            return NoContent();
        }

        /// <summary>
        /// Removes a job post by id (soft delete).
        /// </summary>
        /// <param name="id">Job post id</param>
        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveJobPost(int id)
        {
            var command = new RemoveJobPostCommand { JobPostId = id };
            try
            {
                await mediator.Send(command);
            }
            catch (NotFoundException)
            {
                return NotFound($"Job post with id {id} not found");
            }
            catch (RemoveFailedException)
            {
                return BadRequest($"Failed to remove job post with id {id}");
            }
            return NoContent();
        }
        [HttpGet]
        public async Task<ActionResult<Pagination<SearchJobPostDto>>> SearchJobPosts([FromQuery] JobPostSearchParams jobPostSearchParams)
        {
            var jobPosts = await mediator.Send(new GetSearchJobPostQuery(jobPostSearchParams));
            return ResultWithPagination(jobPosts.Data, jobPosts.Count, jobPostSearchParams.PageIndex, jobPostSearchParams.PageSize);
        }
    }
}
