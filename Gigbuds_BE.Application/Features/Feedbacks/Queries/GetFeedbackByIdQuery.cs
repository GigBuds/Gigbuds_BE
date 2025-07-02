using System;
using Gigbuds_BE.Application.DTOs.Feedbacks;
using MediatR;

namespace Gigbuds_BE.Application.Features.Feedbacks.Queries;

public class GetFeedbackByIdQuery : IRequest<FeedbackDto>
{
    public int FeedbackId { get; set; }
    public GetFeedbackByIdQuery(int feedbackId)
    {
        FeedbackId = feedbackId;
    }
}
