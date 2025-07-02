using System;
using Gigbuds_BE.Application.DTOs.Feedbacks;
using Gigbuds_BE.Application.Interfaces.Repositories;
using Gigbuds_BE.Application.Specifications.Feedbacks;
using Gigbuds_BE.Domain.Entities.Feedbacks;
using MediatR;
using AutoMapper;

namespace Gigbuds_BE.Application.Features.Feedbacks.Queries;

public class GetFeedbackByIdQueryHandler(IUnitOfWork _unitOfWork, IMapper _mapper) : IRequestHandler<GetFeedbackByIdQuery, FeedbackDto>
{
    public async Task<FeedbackDto> Handle(GetFeedbackByIdQuery request, CancellationToken cancellationToken)
    {
        var spec = new GetFeedbackByIdSpecification(request.FeedbackId);
        var feedback = await _unitOfWork.Repository<Feedback>().GetBySpecificationProjectedAsync<FeedbackDto>(spec, _mapper.ConfigurationProvider);
        return feedback;
    }
}
