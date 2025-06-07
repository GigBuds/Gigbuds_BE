using System;
using AutoMapper;
using Gigbuds_BE.Application.DTOs;
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
        CreateMap<JobApplication, JobApplicationResponseDto>()
        .ForMember(dest => dest.AppliedAt, opt => opt.MapFrom(src => src.CreatedAt));

        //Projection
        CreateProjection<JobApplication, JobApplicationForJobPostDto>()
            .ForMember(dest => dest.JobPosition, opt => opt.MapFrom(src => src.JobPost.JobPosition.JobPositionName))
            .ForMember(dest => dest.JobTitle, opt => opt.MapFrom(src => src.JobPost.JobTitle))
            .ForMember(dest => dest.CvUrl, opt => opt.MapFrom(src => src.CvUrl))
            .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.Account.FirstName))
            .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.Account.LastName))
            .ForMember(dest => dest.SkillTags, opt => opt.MapFrom(src => src.Account.SkillTags));


        CreateProjection<JobApplication, JobPostDto>()
            .ForMember(dest => dest.JobPositionId, opt => opt.MapFrom(src => src.JobPost.JobPositionId))
            .ForMember(dest => dest.JobPositionName, opt => opt.MapFrom(src => src.JobPost.JobPosition.JobPositionName))
            .ForMember(dest => dest.JobTitle, opt => opt.MapFrom(src => src.JobPost.JobTitle))
            .ForMember(dest => dest.JobDescription, opt => opt.MapFrom(src => src.JobPost.JobDescription))
            .ForMember(dest => dest.JobRequirement, opt => opt.MapFrom(src => src.JobPost.JobRequirement))
            .ForMember(dest => dest.ExperienceRequirement, opt => opt.MapFrom(src => src.JobPost.ExperienceRequirement))
            .ForMember(dest => dest.Salary, opt => opt.MapFrom(src => src.JobPost.Salary))
            .ForMember(dest => dest.SalaryUnit, opt => opt.MapFrom(src => src.JobPost.SalaryUnit))
            .ForMember(dest => dest.JobLocation, opt => opt.MapFrom(src => src.JobPost.JobLocation))
            .ForMember(dest => dest.ExpireTime, opt => opt.MapFrom(src => src.JobPost.ExpireTime))
            .ForMember(dest => dest.Benefit, opt => opt.MapFrom(src => src.JobPost.Benefit))
            .ForMember(dest => dest.VacancyCount, opt => opt.MapFrom(src => src.JobPost.VacancyCount))
            .ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => src.JobPost.Account.EmployerProfile.CompanyName))
            .ForMember(dest => dest.CompanyLogo, opt => opt.MapFrom(src => src.JobPost.Account.EmployerProfile.CompanyLogo))
            .ForMember(dest => dest.JobSchedule, opt => opt.MapFrom(src => src.JobPost.JobPostSchedule));
    }
}
