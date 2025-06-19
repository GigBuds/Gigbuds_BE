using System;
using Gigbuds_BE.Application.DTOs.Memberships;
using Gigbuds_BE.Domain.Entities.Memberships;

namespace Gigbuds_BE.Application.Interfaces.Services;

public interface IMembershipsService
{
    Task<MembershipResponseDto> CreateMemberShipBenefitsAsync(int accountId, Membership membership);
    Task RevokeMembershipAsync(int accountId, int membershipId);
    Task ScheduleMembershipExpirationAsync(int accountId, Membership membership);
    Task<bool> ProcessMembershipPaymentSuccessAsync(long orderCode);
    Task<bool> CancelMembershipAsync(int accountId, int membershipId);
}
