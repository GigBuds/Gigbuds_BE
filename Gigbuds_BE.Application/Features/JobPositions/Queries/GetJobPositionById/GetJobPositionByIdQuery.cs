using Gigbuds_BE.Application.DTOs.JobPositions;
using MediatR;

namespace Gigbuds_BE.Application.Features.JobPositions.Queries.GetJobPositionById;

public class GetJobPositionByIdQuery : IRequest<JobPositionDto?>
{
    public int JobPositionId { get; set; }

    public GetJobPositionByIdQuery(int jobPositionId)
    {
        JobPositionId = jobPositionId;
    }
} 