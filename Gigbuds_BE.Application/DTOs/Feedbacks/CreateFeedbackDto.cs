using System;
using Gigbuds_BE.Domain.Entities.Feedbacks;

namespace Gigbuds_BE.Application.DTOs.Feedbacks;

public class CreateFeedbackDto
{
    public int JobSeekerId { get; set; }
    public int EmployerId { get; set; }
    public int JobHistoryId { get; set; }
    public int Rating { get; set; }
    public string Comment { get; set; } = string.Empty;
    public FeedbackType FeedbackType { get; set; }
}
