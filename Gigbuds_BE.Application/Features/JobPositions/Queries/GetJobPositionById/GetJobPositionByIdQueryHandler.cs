using AutoMapper;
using Gigbuds_BE.Application.DTOs.JobPositions;
using Gigbuds_BE.Application.Interfaces.Repositories;
using Gigbuds_BE.Application.Specifications.JobPositions;
using Gigbuds_BE.Domain.Entities.Jobs;
using MediatR;

namespace Gigbuds_BE.Application.Features.JobPositions.Queries.GetJobPositionById;

public class GetJobPositionByIdQueryHandler : IRequestHandler<GetJobPositionByIdQuery, JobPositionDto?>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetJobPositionByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<JobPositionDto?> Handle(GetJobPositionByIdQuery request, CancellationToken cancellationToken)
    {
        var jobPosition = await _unitOfWork.Repository<JobPosition>()
            .GetBySpecificationAsync(new JobPositionByIdSpecification(request.JobPositionId));

        return jobPosition == null ? null : _mapper.Map<JobPositionDto>(jobPosition);
    }
} 