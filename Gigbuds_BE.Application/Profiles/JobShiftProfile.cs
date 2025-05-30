using AutoMapper;
using Gigbuds_BE.Application.DTOs.ApplicationUsers;
using Gigbuds_BE.Domain.Entities.Jobs;

namespace Gigbuds_BE.Application.Profiles
{
    public class JobShiftProfile : Profile
    {
        public JobShiftProfile()
        {
            // Entity to DTO mapping
            CreateMap<JobShift, JobShiftDto>()
                .ForMember(dest => dest.DayOfWeek, opt => opt.MapFrom(src => src.DayOfWeek))
                .ForMember(dest => dest.StartTime, opt => opt.MapFrom(src => src.StartTime))
                .ForMember(dest => dest.EndTime, opt => opt.MapFrom(src => src.EndTime));

            // DTO to Entity mapping
            CreateMap<JobShiftDto, JobShift>()
                .ForMember(dest => dest.DayOfWeek, opt => opt.MapFrom(src => src.DayOfWeek))
                .ForMember(dest => dest.StartTime, opt => opt.MapFrom(src => src.StartTime))
                .ForMember(dest => dest.EndTime, opt => opt.MapFrom(src => src.EndTime));

        }
    }
} 