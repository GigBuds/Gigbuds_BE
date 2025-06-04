using Gigbuds_BE.Application.Interfaces.Repositories;
using Gigbuds_BE.Application.Specifications.JobPositions;
using Gigbuds_BE.Domain.Entities.Jobs;
using Gigbuds_BE.Domain.Exceptions;
using MediatR;

namespace Gigbuds_BE.Application.Features.JobPositions.Commands.DeleteJobPosition;

public class DeleteJobPositionCommandHandler : IRequestHandler<DeleteJobPositionCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteJobPositionCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(DeleteJobPositionCommand request, CancellationToken cancellationToken)
    {
        // Get existing job position
        var existingJobPosition = await _unitOfWork.Repository<JobPosition>()
            .GetBySpecificationAsync(new JobPositionByIdSpecification(request.JobPositionId));
        
        if (existingJobPosition == null)
        {
            throw new NotFoundException($"Job position with id {request.JobPositionId} not found.");
        }

        // Soft delete (disable the entity)
        _unitOfWork.Repository<JobPosition>().SoftDelete(existingJobPosition);
        await _unitOfWork.CompleteAsync();
    }
} 