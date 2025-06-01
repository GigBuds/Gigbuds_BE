using System;
using AutoMapper;
using Gigbuds_BE.Application.DTOs.JobApplicationDto;
using Gigbuds_BE.Application.DTOs.JobApplications;
using Gigbuds_BE.Domain.Entities.Jobs;

namespace Gigbuds_BE.Application.Profiles;

public class JobApplicationProfile : Profile
{
    public JobApplicationProfile()
    {
        //Mapping
        CreateMap<JobApplicationDto, JobApplication>();
        CreateMap<JobApplication, JobApplicationResponseDto>();

        //Projection
        CreateProjection<JobApplication, JobApplicationForJobPostDto>()
            .ForMember(dest => dest.JobPosition, opt => opt.MapFrom(src => src.JobPost.JobPosition.JobPositionName))
            .ForMember(dest => dest.JobTitle, opt => opt.MapFrom(src => src.JobPost.JobTitle))
            .ForMember(dest => dest.CvUrl, opt => opt.MapFrom(src => src.CvUrl))
            .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.Account.FirstName))
            .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.Account.LastName))
            .ForMember(dest => dest.SkillTags, opt => opt.MapFrom(src => src.Account.SkillTags));
    }
}
