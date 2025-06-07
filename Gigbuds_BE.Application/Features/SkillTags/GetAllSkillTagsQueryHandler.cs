using System;
using AutoMapper;
using Gigbuds_BE.Application.DTOs.SkillTags;
using Gigbuds_BE.Application.Interfaces.Repositories;
using Gigbuds_BE.Application.Specifications.SkillTags;
using Gigbuds_BE.Domain.Entities.Accounts;
using MediatR;

namespace Gigbuds_BE.Application.Features.SkillTags;

public class GetAllSkillTagsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<GetAllSkillTagsQuery, List<SkillTagDto>>
{
    public async Task<List<SkillTagDto>> Handle(GetAllSkillTagsQuery request, CancellationToken cancellationToken)
    {
        var spec = new GetAllSkillTagsSpecification();
        var skillTags = await unitOfWork.Repository<SkillTag>().GetAllWithSpecificationProjectedAsync<SkillTagDto>(spec, mapper.ConfigurationProvider);
        return skillTags.ToList();
    }
}
