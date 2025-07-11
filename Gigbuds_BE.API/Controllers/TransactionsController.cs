using System;
using Gigbuds_BE.Application.Features.Transactions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Gigbuds_BE.API.Controllers;

public class TransactionsController(IMediator _mediator) : _BaseApiController
{
    [HttpGet]
    public async Task<IActionResult> GetAllTransactions()
    {
        var query = new GetAllTransactionsQuery();
        var result = await _mediator.Send(query);
        return Ok(result);
    }
}
