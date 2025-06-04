using System;
using Gigbuds_BE.Application.DTOs.Memberships;
using Gigbuds_BE.Domain.Entities.Memberships;
using MediatR;

namespace Gigbuds_BE.Application.Features.Memberships.Commands;

public class RegisterMembershipCommand : IRequest<MembershipResponseDto>
{
    public int UserId { get; set; }
    public MembershipType MembershipType { get; set; }
    public int MembershipId { get; set; }
}
