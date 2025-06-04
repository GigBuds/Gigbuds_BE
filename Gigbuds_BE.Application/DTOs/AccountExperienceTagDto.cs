namespace Gigbuds_BE.Application.DTOs
{
    public class AccountExperienceTagDto
    {
        // TODO: add job location
        public required string JobPosition { get; set;  }
        public required DateTime StartDate { get; set; }
        public required DateTime EndDate { get; set; }
    }
}
