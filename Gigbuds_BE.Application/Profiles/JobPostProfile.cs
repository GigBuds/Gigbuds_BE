using AutoMapper;
using Gigbuds_BE.Application.DTOs;
using Gigbuds_BE.Application.DTOs.JobPosts;
using Gigbuds_BE.Application.Features.JobPosts.Commands.CreateJobPost;
using Gigbuds_BE.Domain.Entities.Jobs;

namespace Gigbuds_BE.Application.Profiles
{
    public class JobPostProfile : Profile
    {
        public JobPostProfile()
        {
            // Mapping 
            CreateMap<CreateJobPostCommand, JobPost>()
                .ForMember(dest => dest.JobTitle, opt => opt.MapFrom(src => src.JobTitle))
                .ForMember(dest => dest.JobDescription, opt => opt.MapFrom(src => src.JobDescription))
                .ForMember(dest => dest.JobRequirement, opt => opt.MapFrom(src => src.JobRequirement))
                .ForMember(dest => dest.ExperienceRequirement, opt => opt.MapFrom(src => src.ExperienceRequirement))
                .ForMember(dest => dest.Salary, opt => opt.MapFrom(src => src.Salary))
                .ForMember(dest => dest.SalaryUnit, opt => opt.MapFrom(src => Enum.Parse<SalaryUnit>(src.SalaryUnit)))
                .ForMember(dest => dest.JobLocation, opt => opt.MapFrom(src => src.JobLocation))
                .ForMember(dest => dest.ExpireTime, opt => opt.MapFrom(src => src.ExpireTime))
                .ForMember(dest => dest.Benefit, opt => opt.MapFrom(src => src.Benefit))
                .ForMember(dest => dest.VacancyCount, opt => opt.MapFrom(src => src.VacancyCount))
                .ForMember(dest => dest.IsOutstandingPost, opt => opt.MapFrom(src => src.IsOutstandingPost));

            CreateMap<JobPost, JobPostDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.JobTitle, opt => opt.MapFrom(src => src.JobTitle))
                .ForMember(dest => dest.JobDescription, opt => opt.MapFrom(src => src.JobDescription))
                .ForMember(dest => dest.JobRequirement, opt => opt.MapFrom(src => src.JobRequirement))
                .ForMember(dest => dest.ExperienceRequirement, opt => opt.MapFrom(src => src.ExperienceRequirement))
                .ForMember(dest => dest.Salary, opt => opt.MapFrom(src => src.Salary))
                .ForMember(dest => dest.SalaryUnit, opt => opt.MapFrom(src => src.SalaryUnit.ToString()))
                .ForMember(dest => dest.JobLocation, opt => opt.MapFrom(src => src.JobLocation))
                .ForMember(dest => dest.ExpireTime, opt => opt.MapFrom(src => src.ExpireTime))
                .ForMember(dest => dest.Benefit, opt => opt.MapFrom(src => src.Benefit))
                .ForMember(dest => dest.VacancyCount, opt => opt.MapFrom(src => src.VacancyCount))
                .ForMember(dest => dest.IsOutstandingPost, opt => opt.MapFrom(src => src.IsOutstandingPost))
                .ForMember(dest => dest.JobSchedule, opt => opt.MapFrom(src => src.JobPostSchedule))
                .ForMember(dest => dest.JobPositionName, opt => opt.MapFrom(src => src.JobPosition.JobPositionName))
                .ForMember(dest => dest.JobPositionId, opt => opt.MapFrom(src => src.JobPositionId))
                .ForMember(dest => dest.CompanyLogo, opt => opt.MapFrom(src => src.Account.EmployerProfile.CompanyLogo))
                .ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => src.Account.EmployerProfile.CompanyName));
                
            //Projection
            CreateProjection<JobPost, SearchJobPostDto>()
                .ForMember(dest => dest.CompanyLogo, opt => opt.MapFrom(src => src.Account.EmployerProfile.CompanyLogo))
                .ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => src.Account.EmployerProfile.CompanyName))
                .ForMember(dest => dest.JobPositionName, opt => opt.MapFrom(src => src.JobPosition.JobPositionName))
                .ForMember(dest => dest.JobPositionId, opt => opt.MapFrom(src => src.JobPositionId));
        }
    }
}
