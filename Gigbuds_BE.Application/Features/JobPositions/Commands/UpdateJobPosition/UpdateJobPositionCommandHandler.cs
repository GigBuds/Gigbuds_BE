using AutoMapper;
using Gigbuds_BE.Application.Interfaces.Repositories;
using Gigbuds_BE.Application.Specifications.JobPositions;
using Gigbuds_BE.Domain.Entities.Jobs;
using Gigbuds_BE.Domain.Exceptions;
using MediatR;

namespace Gigbuds_BE.Application.Features.JobPositions.Commands.UpdateJobPosition;

public class UpdateJobPositionCommandHandler : IRequestHandler<UpdateJobPositionCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateJobPositionCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task Handle(UpdateJobPositionCommand request, CancellationToken cancellationToken)
    {
        // Get existing job position
        var existingJobPosition = await _unitOfWork.Repository<JobPosition>()
            .GetBySpecificationAsync(new JobPositionByIdSpecification(request.JobPositionId));
        
        if (existingJobPosition == null)
        {
            throw new NotFoundException($"Job position with id {request.JobPositionId} not found.");
        }

        // Check if another job position with the same name exists
        var duplicateJobPosition = await _unitOfWork.Repository<JobPosition>()
            .GetBySpecificationAsync(new JobPositionByNameSpecification(request.JobPositionName));
        
        if (duplicateJobPosition != null && duplicateJobPosition.Id != request.JobPositionId)
        {
            throw new UpdateFailedException($"Job position with name '{request.JobPositionName}' already exists.");
        }

        // Check if JobType exists
        var jobType = await _unitOfWork.Repository<JobType>()
            .GetBySpecificationAsync(new JobTypeByIdSpecification(request.JobTypeId));
        
        if (jobType == null)
        {
            throw new UpdateFailedException($"Job type with id {request.JobTypeId} does not exist.");
        }

        // Update job position
        existingJobPosition.JobPositionName = request.JobPositionName;
        existingJobPosition.JobTypeId = request.JobTypeId;
        existingJobPosition.UpdatedAt = DateTime.UtcNow;

        _unitOfWork.Repository<JobPosition>().Update(existingJobPosition);
        await _unitOfWork.CompleteAsync();
    }
} 