using System;
using Gigbuds_BE.Application.DTOs.Feedbacks;
using MediatR;

namespace Gigbuds_BE.Application.Features.Feedbacks.CreateFeedback;

public class CreateFeedbackCommand(CreateFeedbackDto createFeedbackDto) : IRequest<CreateFeedbackDto>
{
    public CreateFeedbackDto CreateFeedbackDto { get; set; } = createFeedbackDto;
}
