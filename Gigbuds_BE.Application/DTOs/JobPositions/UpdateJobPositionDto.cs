using System.ComponentModel.DataAnnotations;

namespace Gigbuds_BE.Application.DTOs.JobPositions;

public class UpdateJobPositionDto
{
    [Required]
    [StringLength(255, MinimumLength = 1)]
    public string JobPositionName { get; set; } = string.Empty;
    
    [Required]
    public int JobTypeId { get; set; }
} 