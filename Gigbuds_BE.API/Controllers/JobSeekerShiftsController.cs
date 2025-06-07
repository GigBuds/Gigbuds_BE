using Gigbuds_BE.Application.DTOs.JobSeekerShifts;
using Gigbuds_BE.Application.Features.JobSeekerShifts;
using Gigbuds_BE.Application.Features.JobSeekerShifts.Query;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Gigbuds_BE.API.Controllers;

public class JobSeekerShiftsController : _BaseApiController
{
    private readonly IMediator _mediator;

    public JobSeekerShiftsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPut("update-job-shifts")]
    public async Task<IActionResult> UpdateJobShifts([FromBody] UpdateJobSeekerShiftCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpGet("{accountId}")]
    public async Task<ActionResult<List<JobSeekerShiftResponseDto>>> GetAllJobSeekerShifts([FromRoute] int accountId)
    {
        var result = await _mediator.Send(new GetAllJobSeekerShiftsQuery(accountId));
        return Ok(result);
    }
}
