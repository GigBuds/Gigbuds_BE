using System;
using Gigbuds_BE.Application.DTOs.Memberships;
using MediatR;

namespace Gigbuds_BE.Application.Features.Memberships.Queries;

public class GetAllMembershipQuery : IRequest<List<MembershipDetailResponseDto>>
{

}
