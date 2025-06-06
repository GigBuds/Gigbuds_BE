namespace Gigbuds_BE.Application.DTOs.JobPositions;

public class JobPositionDto
{
    public int Id { get; set; }
    public string JobPositionName { get; set; } = string.Empty;
    public int JobTypeId { get; set; }
    public string JobTypeName { get; set; } = string.Empty;
} 