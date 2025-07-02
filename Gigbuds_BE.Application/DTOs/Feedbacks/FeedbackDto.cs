using Gigbuds_BE.Domain.Entities.Feedbacks;

namespace Gigbuds_BE.Application.DTOs.Feedbacks
{
    public class FeedbackDto
    {
        public int Id { get; set; }
        public string? AccountName { get; set; } = string.Empty;
        public string? AccountAvatar { get; set; } = string.Empty;
        public int EmployerId { get; set; }
        public string? EmployerName { get; set; } = string.Empty;
        public string? CompanyName { get; set; } = string.Empty;
        public string? CompanyLogo { get; set; } = string.Empty;
        public FeedbackType FeedbackType { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public string JobTitle { get; set; } = string.Empty;
    }
} 