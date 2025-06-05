using AutoMapper;
using Gigbuds_BE.Application.DTOs.ApplicationUsers;
using Gigbuds_BE.Application.DTOs.JobRecommendations;
using Gigbuds_BE.Domain.Entities.Jobs;

namespace Gigbuds_BE.Application.Profiles
{
    public class JobPostScheduleProfile : Profile
    {
        public JobPostScheduleProfile()
        {
            // Entity to DTO mapping
            CreateMap<JobPostSchedule, JobRecommendationScheduleDto>()
                .ForMember(dest => dest.ShiftCount, opt => opt.MapFrom(src => src.ShiftCount))
                .ForMember(dest => dest.MinimumShift, opt => opt.MapFrom(src => src.MinimumShift))
                .ForMember(dest => dest.JobShifts, opt => opt.MapFrom(src => src.JobShifts));

            // DTO to Entity mapping
            CreateMap<JobScheduleDto, JobPostSchedule>()
                .ForMember(dest => dest.ShiftCount, opt => opt.MapFrom(src => src.ShiftCount))
                .ForMember(dest => dest.MinimumShift, opt => opt.MapFrom(src => src.MinimumShift))
                .ForMember(dest => dest.JobShifts, opt => opt.MapFrom(src => src.JobShifts))
                .ReverseMap();

        }
    }
} 