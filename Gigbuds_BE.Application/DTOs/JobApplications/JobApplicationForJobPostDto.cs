using System;
using Gigbuds_BE.Application.DTOs.SkillTags;

namespace Gigbuds_BE.Application.DTOs.JobApplications;

public class JobApplicationForJobPostDto
{
    public int Id { get; set; }
    public int JobPostId { get; set; }
    public int AccountId { get; set; }
    public string JobPosition { get; set; } = string.Empty;
    public string JobTitle { get; set; } = string.Empty;
    public string? CvUrl { get; set; }
    public string ApplicationStatus { get; set; } = string.Empty;
    public DateTime AppliedAt { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public IReadOnlyCollection<SkillTagDto> SkillTags { get; set; } = new List<SkillTagDto>();
}
