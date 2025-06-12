using System;
using AutoMapper;
using Gigbuds_BE.Application.DTOs.Memberships;
using Gigbuds_BE.Application.Interfaces.Repositories;
using Gigbuds_BE.Application.Specifications.Memberships;
using Gigbuds_BE.Domain.Entities.Memberships;
using MediatR;

namespace Gigbuds_BE.Application.Features.Memberships.Queries;

public class GetAllMembershipQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<GetAllMembershipQuery, List<MembershipDetailResponseDto>>
{
    public async Task<List<MembershipDetailResponseDto>> Handle(GetAllMembershipQuery request, CancellationToken cancellationToken)
    {
        var spec = new GetAllMembershipSpecification();
        var memberships = await unitOfWork.Repository<Membership>().GetAllWithSpecificationProjectedAsync<MembershipDetailResponseDto>(spec, mapper.ConfigurationProvider);
        return memberships.ToList();
    }
}
