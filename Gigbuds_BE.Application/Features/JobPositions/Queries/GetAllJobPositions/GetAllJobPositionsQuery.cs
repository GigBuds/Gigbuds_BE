using Gigbuds_BE.Application.DTOs.JobPositions;
using MediatR;

namespace Gigbuds_BE.Application.Features.JobPositions.Queries.GetAllJobPositions;

public class GetAllJobPositionsQuery : IRequest<IEnumerable<JobPositionDto>>
{
} 