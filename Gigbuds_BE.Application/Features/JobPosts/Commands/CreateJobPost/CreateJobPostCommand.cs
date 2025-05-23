using Gigbuds_BE.Application.Features.Schedules.Commands.CreateJobPostSchedule;
using System.Text.Json.Serialization;

namespace Gigbuds_BE.Application.Features.JobPosts.Commands.CreateJobPost
{
    public class CreateJobPostCommand
    {
        public required string JobTitle { get; set; } = string.Empty;

        public required string JobDescription { get; set; } = string.Empty;
        public required string JobRequirement { get; set; } = string.Empty;
        public required string ExperienceRequirement { get; set; } = string.Empty;
        public required int Salary { get; set; }
        public required string SalaryUnit { get; set; }
        public required string JobLocation { get; set; } = string.Empty;
        public required DateTime ExpireTime { get; set; }
        public required string Benefit { get; set; } = string.Empty;
        public required int VacancyCount { get; set; }
        public required bool IsOutstandingPost { get; set; }
        [JsonPropertyName("JobSchedule")]
        public required CreateJobPostScheduleCommand ScheduleCommand { get; set; }
    }
}
