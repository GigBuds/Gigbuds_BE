using Gigbuds_BE.Application.Features.Schedules.Commands.CreateJobPostSchedule;
using System.Text.Json.Serialization;
using MediatR;

namespace Gigbuds_BE.Application.Features.JobPosts.Commands.CreateJobPost
{
    public class CreateJobPostCommand : IRequest<int>
    {
        public required int AccountId { get; set; }
        public required string JobTitle { get; set; }
        public required int AgeRequirement { get; set; }
        public required string JobDescription { get; set; }
        public required string JobRequirement { get; set; }
        public required string ExperienceRequirement { get; set; }
        public required int Salary { get; set; }
        public required string SalaryUnit { get; set; }
        public required string JobLocation { get; set; }
        public required DateTime ExpireTime { get; set; }
        public required string Benefit { get; set; }
        public required int VacancyCount { get; set; }
        public required string DistrictCode { get; set; }
        public required string ProvinceCode { get; set; }
        public required bool IsOutstandingPost { get; set; }
        public required int JobPositionId { get; set; }
        [JsonPropertyName("JobSchedule")]
        public required CreateJobPostScheduleCommand ScheduleCommand { get; set; }
    }
}
