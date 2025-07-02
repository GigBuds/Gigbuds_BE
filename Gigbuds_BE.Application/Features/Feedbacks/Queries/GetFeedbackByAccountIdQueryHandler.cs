using System;
using Gigbuds_BE.Application.DTOs.Feedbacks;
using Gigbuds_BE.Application.Interfaces.Repositories;
using Gigbuds_BE.Application.Specifications.Feedbacks;
using Gigbuds_BE.Domain.Entities.Feedbacks;
using MediatR;
using AutoMapper;

namespace Gigbuds_BE.Application.Features.Feedbacks.Queries;

public class GetFeedbackByAccountIdQueryHandler(IUnitOfWork _unitOfWork, IMapper _mapper) : IRequestHandler<GetFeedbackByAccountIdQuery, List<FeedbackDto>>
{
    public async Task<List<FeedbackDto>> Handle(GetFeedbackByAccountIdQuery request, CancellationToken cancellationToken)
    {
        var spec = new GetFeedbackByAccountIdSpecification(request.AccountId, request.FeedbackType);
        var feedbacks = await _unitOfWork.Repository<Feedback>().GetAllWithSpecificationProjectedAsync<FeedbackDto>(spec, _mapper.ConfigurationProvider);
        return feedbacks.ToList();
    }
}
