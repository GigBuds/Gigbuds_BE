using System;
using Gigbuds_BE.Application.DTOs.Feedbacks;
using Gigbuds_BE.Application.Features.Feedbacks.CreateFeedback;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Gigbuds_BE.API.Controllers;

public class FeedbacksController(IMediator mediator) : _BaseApiController
{
    [HttpPost]
    public async Task<IActionResult> CreateFeedback(CreateFeedbackDto createFeedbackDto)
    {
        try {
            var command = new CreateFeedbackCommand(createFeedbackDto);
            var feedback = await mediator.Send(command);
            return Ok(new { success = true, data = feedback });
        } catch (Exception ex) {
            return BadRequest(new { success = false, error = ex.Message });
        }
    }
}
