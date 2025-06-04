using MediatR;

namespace Gigbuds_BE.Application.Features.JobPositions.Commands.CreateJobPosition;

public class CreateJobPositionCommand : IRequest<int>
{
    public string JobPositionName { get; set; } = string.Empty;
    public int JobTypeId { get; set; }
} 