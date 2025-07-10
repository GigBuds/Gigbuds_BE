using System;
using Gigbuds_BE.Application.Features.Accounts;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gigbuds_BE.API.Controllers;

public class AccountsController(IMediator _mediator) : _BaseApiController
{
    [HttpGet()]    
    [AllowAnonymous]
    public async Task<IActionResult> GetAllAccount() {
        var query = new GetAllAccountQuery();
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpPut("banned/{id}")]
    public async Task<IActionResult> BanAccount(int id, bool isEnabled) {
        var command = new BanAccountCommand(id, isEnabled);
        var result = await _mediator.Send(command);
        return Ok(result);
    }
}
