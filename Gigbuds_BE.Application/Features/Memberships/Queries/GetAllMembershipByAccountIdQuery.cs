using System;
using Gigbuds_BE.Application.DTOs.Memberships;
using MediatR;

namespace Gigbuds_BE.Application.Features.Memberships.Queries;

public class GetAllMembershipByAccountIdQuery : IRequest<List<MemberShipByIdDto>>
{
    public int AccountId { get; set; }
    public GetAllMembershipByAccountIdQuery(int accountId)
    {
        AccountId = accountId;
    }
}
