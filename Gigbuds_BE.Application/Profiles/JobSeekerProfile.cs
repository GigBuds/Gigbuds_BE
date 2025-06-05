using AutoMapper;
using Gigbuds_BE.Application.DTOs;
using Gigbuds_BE.Application.DTOs.ApplicationUsers;
using Gigbuds_BE.Application.DTOs.SkillTags;
using Gigbuds_BE.Application.Features.Embedding.JobSeekerEmbedding;
using Gigbuds_BE.Domain.Entities.Accounts;
using Gigbuds_BE.Domain.Entities.Identity;

namespace Gigbuds_BE.Application.Profiles
{
    public class JobSeekerProfile : Profile
    {
        public JobSeekerProfile()
        {
            CreateMap<ApplicationUser, JobSeekerEmbeddingRequest>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Dob, opt => opt.MapFrom(src => DateOnly.FromDateTime(src.Dob)))
                .ForMember(dest => dest.IsMale, opt => opt.MapFrom(src => src.IsMale))
                .ForMember(dest => dest.IsEnabled, opt => opt.MapFrom(src => src.IsEnabled))
                .ForMember(dest => dest.Location, opt => opt.MapFrom(src => src.CurrentLocation ?? string.Empty))
                .ForMember(dest => dest.SkillTags, opt => opt.MapFrom(src => src.SkillTags))
                .ForMember(dest => dest.EducationalLevels, opt => opt.MapFrom(src => src.EducationalLevels))
                .ForMember(dest => dest.AccountExperienceTags, opt => opt.MapFrom(src => src.AccountExperienceTags));

            CreateMap<SkillTag, SkillTagDto>()
                .ReverseMap();

            CreateMap<EducationalLevel, EducationalLevelDto>()
                .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.StartDate.GetValueOrDefault()))
                .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => src.EndDate.GetValueOrDefault()))
                .ReverseMap();

            CreateMap<AccountExperienceTag, AccountExperienceTagDto>()
                .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.StartTime.GetValueOrDefault()))
                .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => src.EndTime.GetValueOrDefault()))
                .ReverseMap();
        }
    }
}
