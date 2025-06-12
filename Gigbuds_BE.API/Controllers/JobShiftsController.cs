using System;
using Gigbuds_BE.Application.Features.JobShifts;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Gigbuds_BE.API.Controllers;

public class JobShiftsController : _BaseApiController
{
    private readonly IMediator _mediator;

    public JobShiftsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPut]
    public async Task<IActionResult> UpdateJobShift([FromBody] UpdateJobShiftCommand updateJobShiftCommand)
    {
        await _mediator.Send(updateJobShiftCommand);
        return NoContent();
    }
    
}
