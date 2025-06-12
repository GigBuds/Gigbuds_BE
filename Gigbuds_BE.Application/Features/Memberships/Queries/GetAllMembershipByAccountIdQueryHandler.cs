using System;
using AutoMapper;
using Gigbuds_BE.Application.DTOs.Memberships;
using Gigbuds_BE.Application.Interfaces.Repositories;
using Gigbuds_BE.Application.Specifications.Memberships;
using Gigbuds_BE.Domain.Entities.Memberships;
using MediatR;

namespace Gigbuds_BE.Application.Features.Memberships.Queries;

public class GetAllMembershipByAccountIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<GetAllMembershipByAccountIdQuery, List<MemberShipByIdDto>>
{
    public async Task<List<MemberShipByIdDto>> Handle(GetAllMembershipByAccountIdQuery request, CancellationToken cancellationToken)
    {
        var spec = new GetAllMembershipByAccountIdSpecification(request.AccountId);
        var memberships = await unitOfWork.Repository<AccountMembership>().GetAllWithSpecificationProjectedAsync<MemberShipByIdDto>(spec, mapper.ConfigurationProvider);
        return memberships.ToList();
    }
}
