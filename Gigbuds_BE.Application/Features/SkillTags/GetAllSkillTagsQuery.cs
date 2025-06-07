using System;
using Gigbuds_BE.Application.DTOs.SkillTags;
using MediatR;

namespace Gigbuds_BE.Application.Features.SkillTags;

public class GetAllSkillTagsQuery : IRequest<List<SkillTagDto>>
{
}
