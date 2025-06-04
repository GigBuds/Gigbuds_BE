using Gigbuds_BE.Domain.Entities.Accounts;

namespace Gigbuds_BE.Application.DTOs
{
    public class EducationalLevelDto
    {
        public required string Major { get; set; }
        public required string SchoolName { get; set; }
        public EducationalLevelType Level { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
