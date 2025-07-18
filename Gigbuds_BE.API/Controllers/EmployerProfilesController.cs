using System;
using Gigbuds_BE.Application.Features.Accounts.EmployerProfiles.Commands;
using Gigbuds_BE.Application.Features.Accounts.EmployerProfiles.Queries;
using Gigbuds_BE.Application.Features.Accounts.JobSeekers.Queries.GetEmployerByName;
using Gigbuds_BE.Application.Specifications.ApplicationUsers;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Gigbuds_BE.API.Controllers;

public class EmployerProfilesController(IMediator mediator) : _BaseApiController
{
    [HttpGet("{id}")]
    public async Task<IActionResult> GetEmployerProfile(int id)
    {
        try
        {
            var query = new GetEmployerProfileQuery(id);
            var result = await mediator.Send(query);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = "Failed to get employer profile", error = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateEmployerProfile(int id, [FromForm] UpdateEmployerProfileCommand command)
    {
        try
        {
            command.AccountId = id;
            var result = await mediator.Send(command);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = "Failed to update employer profile", error = ex.Message });
        }
    }

    [HttpGet("names")]
    public async Task<IActionResult> GetEmployerNames([FromQuery] EmployerGetByNameQueryParams employerGetByNameQueryParams)
    {
        try
        {
            var result = await mediator.Send(new GetEmployerByNameQuery(employerGetByNameQueryParams));
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = "Failed to get employer names", error = ex.Message });
        }
    }
}
