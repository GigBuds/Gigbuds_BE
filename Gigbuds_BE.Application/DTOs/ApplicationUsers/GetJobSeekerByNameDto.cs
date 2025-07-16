namespace Gigbuds_BE.Application.DTOs.ApplicationUsers
{
    public class GetJobSeekerByNameDto
    {
        public int UserId { get; set; }
        public required string FullName { get; set; } = string.Empty;
        public required string Avatar { get; set; } = string.Empty;
    }
}
