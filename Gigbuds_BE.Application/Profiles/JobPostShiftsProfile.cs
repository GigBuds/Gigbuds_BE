using AutoMapper;
using Gigbuds_BE.Application.DTOs;
using Gigbuds_BE.Application.DTOs.JobPosts;
using Gigbuds_BE.Domain.Entities.Jobs;

namespace Gigbuds_BE.Application.Profiles
{
    public class JobPostShiftsProfile : Profile
    {
        public JobPostShiftsProfile()
        {
            //Entity to DTO mapping
            CreateMap<JobShift, JobPostShiftForRecommendDto>()
                .ForMember(dest => dest.DayOfWeek, opt => opt.MapFrom(src => src.DayOfWeek))
                .ForMember(dest => dest.StartTime, opt => opt.MapFrom(src => src.StartTime))
                .ForMember(dest => dest.EndTime, opt => opt.MapFrom(src => src.EndTime));

            // Entity to DTO mapping
            CreateProjection<JobShift, JobPostShiftsDto>()
                .ForMember(dest => dest.DayOfWeek, opt => opt.MapFrom(src => src.DayOfWeek))
                .ForMember(dest => dest.StartTime, opt => opt.MapFrom(src => src.StartTime))
                .ForMember(dest => dest.EndTime, opt => opt.MapFrom(src => src.EndTime));
        }
    }
}