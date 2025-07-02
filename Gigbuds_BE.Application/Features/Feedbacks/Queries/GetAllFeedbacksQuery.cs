using System;
using Gigbuds_BE.Application.DTOs.Feedbacks;
using Gigbuds_BE.Domain.Entities.Feedbacks;
using MediatR;

namespace Gigbuds_BE.Application.Features.Feedbacks.Queries;

public class GetAllFeedbacksQuery : IRequest<List<FeedbackDto>>
{
    public FeedbackType FeedbackType { get; set; }
    public GetAllFeedbacksQuery(FeedbackType feedbackType)
    {
        FeedbackType = feedbackType;
    }
}
