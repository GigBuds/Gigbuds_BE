using AutoMapper;
using Gigbuds_BE.Application.DTOs;
using Gigbuds_BE.Application.DTOs.ApplicationUsers;
using Gigbuds_BE.Application.DTOs.SkillTags;
using Gigbuds_BE.Application.Features.Accounts.JobSeekers.Commands.EditJobSeeker;
using Gigbuds_BE.Application.Features.Embedding.JobSeekerEmbedding;
using Gigbuds_BE.Domain.Entities.Accounts;
using Gigbuds_BE.Domain.Entities.Identity;
using Gigbuds_BE.Domain.Entities.Schedule;

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

            CreateMap<JobSeekerShift, JobSeekerShiftsDto>()
                .ForMember(dest => dest.JobSeekerId, opt => opt.MapFrom(src => src.JobSeekerSchedule.Account.Id))
                .ReverseMap();

            // Map ApplicationUser to JobSeekerDetailDto
            CreateMap<ApplicationUser, JobSeekerDetailDto>()
                .ForMember(dest => dest.Dob, opt => opt.MapFrom(src => DateOnly.FromDateTime(src.Dob)))
                .ForMember(dest => dest.SkillTags, opt => opt.MapFrom(src => src.SkillTags))
                .ForMember(dest => dest.EducationalLevels, opt => opt.MapFrom(src => src.EducationalLevels))
                .ForMember(dest => dest.AccountExperienceTags, opt => opt.MapFrom(src => src.AccountExperienceTags))
                .ForMember(dest => dest.JobSeekerShifts, opt => opt.Ignore()) // Handled manually in handler
                .ForMember(dest => dest.Feedbacks, opt => opt.Ignore()) // Handled manually in handler
                .ForMember(dest => dest.AverageRating, opt => opt.Ignore()) // Calculated in handler
                .ForMember(dest => dest.TotalFeedbacks, opt => opt.Ignore()) // Calculated in handler
                .ForMember(dest => dest.FollowerCount, opt => opt.Ignore()); // Calculated in handler

            // Map EditJobSeekerDto to EditJobSeekerCommand
            CreateMap<EditJobSeekerDto, EditJobSeekerCommand>();

            CreateMap<ApplicationUser, GetJobSeekerByNameDto>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FirstName + " " + src.LastName))
                .ForMember(dest => dest.Avatar, opt => opt.MapFrom(src => src.AvatarUrl ?? string.Empty));
        }
    }
}
