using AutoMapper;
using Gigbuds_BE.Application.DTOs;
using Gigbuds_BE.Domain.Entities.Schedule;

namespace Gigbuds_BE.Application.Profiles
{
    internal class JobSeekerShiftsProfile : Profile
    {
        public JobSeekerShiftsProfile()
        {
            // Entity to DTO mapping
            CreateProjection<JobSeekerShift, JobSeekerShiftsDto>()
                .ForMember(dest => dest.JobSeekerId, opt => opt.MapFrom(src => src.JobSeekerSchedule.Account.Id))
                .ForMember(dest => dest.DayOfWeek, opt => opt.MapFrom(src => src.DayOfWeek))
                .ForMember(dest => dest.StartTime, opt => opt.MapFrom(src => src.StartTime))
                .ForMember(dest => dest.EndTime, opt => opt.MapFrom(src => src.EndTime));
        }
    }
}
