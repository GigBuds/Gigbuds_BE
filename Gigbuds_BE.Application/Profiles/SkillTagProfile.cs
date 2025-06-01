using System;
using AutoMapper;
using Gigbuds_BE.Application.DTOs.SkillTags;
using Gigbuds_BE.Domain.Entities.Accounts;

namespace Gigbuds_BE.Application.Profiles;

public class SkillTagProfile : Profile
{
    public SkillTagProfile()
    {
        CreateMap<SkillTag, SkillTagDto>()
        .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
        .ForMember(dest => dest.SkillName, opt => opt.MapFrom(src => src.SkillName));
    }
}
