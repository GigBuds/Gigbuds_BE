using Gigbuds_BE.Application.DTOs.ApplicationUsers;

namespace Gigbuds_BE.Application.DTOs
{
    public class JobPostDto
    {
        public int Id { get; set; }
        public required string CompanyName { get; set; }
        public required string CompanyLogo { get; set; }
        public required string AccountId { get; set; }
        public required string JobTitle { get; set; }
        public required string JobDescription { get; set; }
        public required string JobRequirement { get; set; }
        public required int AgeRequirement { get; set; }
        public required string ExperienceRequirement { get; set; }
        public required int Salary { get; set; }
        public required string SalaryUnit { get; set; }
        public required string JobLocation { get; set; }
        public required DateTime ExpireTime { get; set; }
        public required string Benefit { get; set; }
        public required int VacancyCount { get; set; }
        public required bool IsOutstandingPost { get; set; }
        public required JobScheduleDto JobSchedule { get; set; }
        public required string JobPositionName { get; set; }
        public required int JobPositionId { get; set; }
    }
}