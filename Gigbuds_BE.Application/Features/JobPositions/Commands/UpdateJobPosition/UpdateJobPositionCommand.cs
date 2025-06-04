using MediatR;
using System.Text.Json.Serialization;

namespace Gigbuds_BE.Application.Features.JobPositions.Commands.UpdateJobPosition;

public class UpdateJobPositionCommand : IRequest
{
    [JsonIgnore]
    public int JobPositionId { get; set; }
    public string JobPositionName { get; set; } = string.Empty;
    public int JobTypeId { get; set; }
} 