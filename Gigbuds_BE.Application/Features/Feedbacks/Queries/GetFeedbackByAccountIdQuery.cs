using System;
using Gigbuds_BE.Application.DTOs.Feedbacks;
using Gigbuds_BE.Domain.Entities.Feedbacks;
using MediatR;

namespace Gigbuds_BE.Application.Features.Feedbacks.Queries;

public class GetFeedbackByAccountIdQuery : IRequest<List<FeedbackDto>>
{
    public int AccountId { get; set; }
    public FeedbackType FeedbackType { get; set; }
    public GetFeedbackByAccountIdQuery(int accountId, FeedbackType feedbackType)
    {
        AccountId = accountId;
        FeedbackType = feedbackType;
    }
}
