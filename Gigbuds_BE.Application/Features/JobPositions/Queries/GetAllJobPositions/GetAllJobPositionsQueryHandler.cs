using AutoMapper;
using Gigbuds_BE.Application.DTOs.JobPositions;
using Gigbuds_BE.Application.Interfaces.Repositories;
using Gigbuds_BE.Application.Specifications.JobPositions;
using Gigbuds_BE.Domain.Entities.Jobs;
using MediatR;

namespace Gigbuds_BE.Application.Features.JobPositions.Queries.GetAllJobPositions;

public class GetAllJobPositionsQueryHandler : IRequestHandler<GetAllJobPositionsQuery, IEnumerable<JobPositionDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAllJobPositionsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<JobPositionDto>> Handle(GetAllJobPositionsQuery request, CancellationToken cancellationToken)
    {
        var jobPositions = await _unitOfWork.Repository<JobPosition>()
            .GetAllWithSpecificationAsync(new AllJobPositionsSpecification());

        return _mapper.Map<IEnumerable<JobPositionDto>>(jobPositions);
    }
} 