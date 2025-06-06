using AutoMapper;
using Gigbuds_BE.Application.DTOs.JobPositions;
using Gigbuds_BE.Application.Features.JobPositions.Commands.CreateJobPosition;
using Gigbuds_BE.Application.Features.JobPositions.Commands.UpdateJobPosition;
using Gigbuds_BE.Domain.Entities.Jobs;

namespace Gigbuds_BE.Application.Profiles;

public class JobPositionProfile : Profile
{
    public JobPositionProfile()
    {
        // Command to Entity mappings
        CreateMap<CreateJobPositionCommand, JobPosition>();
        CreateMap<UpdateJobPositionCommand, JobPosition>();

        // Entity to DTO mappings
        CreateMap<JobPosition, JobPositionDto>()
            .ForMember(dest => dest.JobTypeName, opt => opt.MapFrom(src => src.JobType != null ? src.JobType.JobTypeName : string.Empty));

        // DTO to Command mappings
        CreateMap<CreateJobPositionDto, CreateJobPositionCommand>();
        CreateMap<UpdateJobPositionDto, UpdateJobPositionCommand>();
    }
} 