using System;
using Gigbuds_BE.Application.DTOs.SkillTags;
using Gigbuds_BE.Application.Features.SkillTags;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Gigbuds_BE.API.Controllers;

public class SkillTagsController : _BaseApiController
{
    private readonly IMediator _mediator;

    public SkillTagsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<List<SkillTagDto>>> GetAllSkillTags()
    {
        var result = await _mediator.Send(new GetAllSkillTagsQuery());
        return Ok(result);
    }
    
}
