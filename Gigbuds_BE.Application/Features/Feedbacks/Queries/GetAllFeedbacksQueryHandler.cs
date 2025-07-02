using System;
using Gigbuds_BE.Application.DTOs.Feedbacks;
using Gigbuds_BE.Application.Interfaces.Repositories;
using Gigbuds_BE.Application.Specifications.Feedbacks;
using Gigbuds_BE.Domain.Entities.Feedbacks;
using AutoMapper;
using MediatR;

namespace Gigbuds_BE.Application.Features.Feedbacks.Queries;

public class GetAllFeedbacksQueryHandler(IUnitOfWork _unitOfWork, IMapper _mapper) : IRequestHandler<GetAllFeedbacksQuery, List<FeedbackDto>>
{
    public async Task<List<FeedbackDto>> Handle(GetAllFeedbacksQuery request, CancellationToken cancellationToken)
    {
        var spec = new GetAllFeedbacksSpecification(request.FeedbackType);
        var feedbacks = await _unitOfWork.Repository<Feedback>().GetAllWithSpecificationProjectedAsync<FeedbackDto>(spec, _mapper.ConfigurationProvider);
        return feedbacks.ToList();
    }
}
