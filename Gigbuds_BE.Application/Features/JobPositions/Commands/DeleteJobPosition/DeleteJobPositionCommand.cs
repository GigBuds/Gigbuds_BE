using MediatR;

namespace Gigbuds_BE.Application.Features.JobPositions.Commands.DeleteJobPosition;

public class DeleteJobPositionCommand : IRequest
{
    public int JobPositionId { get; set; }
} 