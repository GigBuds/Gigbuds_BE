using AutoMapper;
using Gigbuds_BE.Application.DTOs.ApplicationUsers;
using Gigbuds_BE.Application.DTOs.Paging;
using Gigbuds_BE.Application.Features.Accounts.JobSeekers.Commands.EditJobSeeker;
using Gigbuds_BE.Application.Features.Accounts.JobSeekers.Queries.GetJobSeekerById;
using Gigbuds_BE.Application.Features.Accounts.JobSeekers.Queries.GetJobSeekerByName;
using Gigbuds_BE.Application.Features.Accounts.JobSeekers.Queries.GetJobSeekerDetail;
using Gigbuds_BE.Application.Features.Accounts.JobSeekers.Queries.GetJobSeekerLocations;
using Gigbuds_BE.Application.Specifications.ApplicationUsers;
using Gigbuds_BE.Domain.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gigbuds_BE.API.Controllers
{
    [Authorize]
    public class JobSeekersController : _BaseApiController
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public JobSeekersController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpGet("names")] 
        public async Task<ActionResult<PagedResultDto<JobSeekerDto>>> GetJobSeekers([FromQuery] JobSeekerGetByNameQueryParams jobSeekerGetByNameQueryParams)
        {
            try
            {
                var result = await _mediator.Send(new GetJobSeekerByNameQuery(jobSeekerGetByNameQueryParams));
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "An error occurred while retrieving job seekers", details = ex.Message });
            }
        }

        /// <summary>
        /// Get detailed job seeker information including experience, skills, education, schedule, feedbacks and follower count
        /// </summary>
        /// <param name="id">Job seeker ID</param>
        /// <returns>Detailed job seeker information</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(JobSeekerDetailDto), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<JobSeekerDetailDto>> GetJobSeekerDetail(int id)
        {
            try
            {
                var query = new GetJobSeekerDetailQuery { JobSeekerId = id };
                var result = await _mediator.Send(query);
                return Ok(result);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "An error occurred while retrieving job seeker details", details = ex.Message });
            }
        }

        /// <summary>
        /// Edit job seeker information including experience, skills, education and personal information
        /// </summary>
        /// <param name="id">Job seeker ID</param>
        /// <param name="editJobSeekerDto">Updated job seeker information</param>
        /// <returns>No content on success</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> EditJobSeeker(int id, [FromBody] EditJobSeekerCommand editJobSeekerCommand)
        {
            if (editJobSeekerCommand == null)
                return BadRequest("Request body is required");

            try
            {
                editJobSeekerCommand.JobSeekerId = id;
                
                await _mediator.Send(editJobSeekerCommand);
                return NoContent();
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "An error occurred while updating job seeker information", details = ex.Message });
            }
        }

        [HttpGet("location/{id}")]
        public async Task<IActionResult> GetLocation(int id)
        {
            try
            {
                var query = new GetLocationQuery { JobSeekerId = id };
                var result = await _mediator.Send(query);
                return Ok(new {
                    success = true,
                    data = result,
                    error = new {
                        message = "",
                        code = ""
                    }
                });
            }
            catch (NotFoundException ex)
            {
                return NotFound(new {
                    success = false,
                    data = "",
                    error = new {
                        message = ex.Message,
                        code = "NOT_FOUND"
                    }
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new {
                    success = false,
                    data = "",
                    error = new {
                        message = ex.Message,
                        code = "INTERNAL_SERVER_ERROR"
                    }
                });
            }
        }

    }
}
