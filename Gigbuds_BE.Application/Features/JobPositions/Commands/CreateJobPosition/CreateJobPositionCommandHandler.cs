using AutoMapper;
using Gigbuds_BE.Application.Interfaces.Repositories;
using Gigbuds_BE.Application.Specifications.JobPositions;
using Gigbuds_BE.Domain.Entities.Jobs;
using Gigbuds_BE.Domain.Exceptions;
using MediatR;

namespace Gigbuds_BE.Application.Features.JobPositions.Commands.CreateJobPosition;

public class CreateJobPositionCommandHandler : IRequestHandler<CreateJobPositionCommand, int>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateJobPositionCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<int> Handle(CreateJobPositionCommand request, CancellationToken cancellationToken)
    {
        // Check if JobPositionName already exists
        var existingJobPosition = await _unitOfWork.Repository<JobPosition>()
            .GetBySpecificationAsync(new JobPositionByNameSpecification(request.JobPositionName));
        
        if (existingJobPosition != null)
        {
            throw new CreateFailedException($"Job position with name '{request.JobPositionName}' already exists.");
        }

        // Check if JobType exists
        var jobType = await _unitOfWork.Repository<JobType>()
            .GetBySpecificationAsync(new JobTypeByIdSpecification(request.JobTypeId));
        
        if (jobType == null)
        {
            throw new CreateFailedException($"Job type with id {request.JobTypeId} does not exist.");
        }

        var jobPosition = _mapper.Map<JobPosition>(request);
        jobPosition.CreatedAt = DateTime.UtcNow;
        jobPosition.UpdatedAt = DateTime.UtcNow;
        jobPosition.IsEnabled = true;

        var createdJobPosition = _unitOfWork.Repository<JobPosition>().Insert(jobPosition);
        await _unitOfWork.CompleteAsync();

        return createdJobPosition.Id;
    }
} 