using AutoMapper;
using Gigbuds_BE.Application.DTOs;
using Gigbuds_BE.Application.DTOs.ApplicationUsers;
using Gigbuds_BE.Application.DTOs.SkillTags;
using Gigbuds_BE.Domain.Entities.Accounts;
using Gigbuds_BE.Domain.Entities.Identity;

namespace Gigbuds_BE.Application.Profiles
{
    public class JobSeekerProfile : Profile
    {
        public JobSeekerProfile()
        {
            CreateMap<ApplicationUser, JobSeekerDto>()
                .ForMember(dest => dest.accountExperienceTags, opt => opt.MapFrom(src => src.AccountExperienceTags))
                .ForMember(dest => dest.SkillTags, opt => opt.MapFrom(src => src.SkillTags))
                .ForMember(dest => dest.EducationalLevels, opt => opt.MapFrom(src => src.EducationalLevels));
            
            CreateMap<SkillTag, SkillTagDto>()
                .ReverseMap();
            
            CreateMap<EducationalLevel, EducationalLevelDto>()
                .ReverseMap();
        }
    }
}
