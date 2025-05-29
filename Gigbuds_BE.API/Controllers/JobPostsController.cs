using Gigbuds_BE.API.Helpers.RequestHelpers;
using Gigbuds_BE.Application.DTOs.JobPosts;
using Gigbuds_BE.Application.Features.JobPosts.Commands.CreateJobPost;
using Gigbuds_BE.Application.Specifications.JobPosts;
using Gigbuds_BE.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Wolverine;
using MediatR;
using Gigbuds_BE.Application.Features.JobPosts.Queries.GetSearchJobPosts;

namespace Gigbuds_BE.API.Controllers
{
    public class JobPostsController(IMessageBus messageBus, IMediator mediator) : _BaseApiController
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

        [HttpGet]
        public async Task<ActionResult<Pagination<SearchJobPostDto>>> SearchJobPosts([FromQuery] JobPostSearchParams jobPostSearchParams)
        {
            var jobPosts = await mediator.Send(new GetSearchJobPostQuery(jobPostSearchParams));
            return ResultWithPagination(jobPosts.Data, jobPosts.Count, jobPostSearchParams.PageIndex, jobPostSearchParams.PageSize);
        }
    }
}
