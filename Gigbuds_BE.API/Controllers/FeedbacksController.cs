using System;
using Gigbuds_BE.Application.DTOs.Feedbacks;
using Gigbuds_BE.Application.Features.Feedbacks.CreateFeedback;
using Gigbuds_BE.Application.Features.Feedbacks.Queries;
using Gigbuds_BE.Domain.Entities.Feedbacks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using OpenAI.Audio;

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

    [HttpGet]
    public async Task<IActionResult> GetAllFeedbacks(FeedbackType feedbackType)
    {
        var feedbacks = await mediator.Send(new GetAllFeedbacksQuery(feedbackType));
        return Ok(new { success = true, data = feedbacks });
    }

    [HttpGet("account/{id}")]
    public async Task<IActionResult> GetFeedbackByAccountId(int id, FeedbackType feedbackType)
    {
        var feedback = await mediator.Send(new GetFeedbackByAccountIdQuery(id, feedbackType));
        return Ok(new { success = true, data = feedback });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetFeedbackById(int id)
    {
        var feedback = await mediator.Send(new GetFeedbackByIdQuery(id));
        return Ok(new { success = true, data = feedback });
    }
}
