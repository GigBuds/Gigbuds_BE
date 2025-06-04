using AutoMapper;
using Gigbuds_BE.Application.DTOs.JobPositions;
using Gigbuds_BE.Application.Features.JobPositions.Commands.CreateJobPosition;
using Gigbuds_BE.Application.Features.JobPositions.Commands.UpdateJobPosition;
using Gigbuds_BE.Application.Features.JobPositions.Commands.DeleteJobPosition;
using Gigbuds_BE.Application.Features.JobPositions.Queries.GetAllJobPositions;
using Gigbuds_BE.Application.Features.JobPositions.Queries.GetJobPositionById;
using Gigbuds_BE.Domain.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gigbuds_BE.API.Controllers
{
    [Authorize]
    public class JobPositionsController(IMediator mediator, IMapper mapper) : _BaseApiController
    {
        /// <summary>
        /// Creates a new job position.
        /// </summary>
        /// <param name="createJobPositionDto">The data to create a job position.</param>
        /// <returns>The id of the created job position.</returns>
        [HttpPost]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<ActionResult<int>> CreateJobPosition([FromBody] CreateJobPositionDto createJobPositionDto)
        {
            if (createJobPositionDto == null)
                return BadRequest("Request body is required");

            var command = mapper.Map<CreateJobPositionCommand>(createJobPositionDto);
            
            try
            {
                var createdJobPositionId = await mediator.Send(command);
                return Created(string.Empty, createdJobPositionId);
            }
            catch (CreateFailedException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Retrieves all job positions.
        /// </summary>
        /// <returns>List of JobPositionDto</returns>
        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(typeof(IEnumerable<JobPositionDto>), 200)]
        public async Task<ActionResult<IEnumerable<JobPositionDto>>> GetAllJobPositions()
        {
            var jobPositions = await mediator.Send(new GetAllJobPositionsQuery());
            return Ok(jobPositions);
        }

        /// <summary>
        /// Retrieves a job position by id.
        /// </summary>
        /// <param name="id">Job position id</param>
        /// <returns>JobPositionDto</returns>
        [HttpGet("{id}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(JobPositionDto), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<JobPositionDto>> GetJobPositionById(int id)
        {
            var jobPosition = await mediator.Send(new GetJobPositionByIdQuery(id));
            
            if (jobPosition == null)
                return NotFound($"Job position with id {id} not found");

            return Ok(jobPosition);
        }

        /// <summary>
        /// Updates a job position by id.
        /// </summary>
        /// <param name="id">Job position id</param>
        /// <param name="updateJobPositionDto">The data to update the job position</param>
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> UpdateJobPosition(int id, [FromBody] UpdateJobPositionDto updateJobPositionDto)
        {
            if (updateJobPositionDto == null)
                return BadRequest("Request body is required");

            var command = mapper.Map<UpdateJobPositionCommand>(updateJobPositionDto);
            command.JobPositionId = id;
            
            try
            {
                await mediator.Send(command);
                return NoContent();
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (UpdateFailedException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Deletes a job position by id (soft delete).
        /// </summary>
        /// <param name="id">Job position id</param>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteJobPosition(int id)
        {
            var command = new DeleteJobPositionCommand { JobPositionId = id };
            
            try
            {
                await mediator.Send(command);
                return NoContent();
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
