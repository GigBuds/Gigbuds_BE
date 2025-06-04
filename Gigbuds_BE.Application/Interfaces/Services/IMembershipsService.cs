using System;
using Gigbuds_BE.Domain.Entities.Memberships;

namespace Gigbuds_BE.Application.Interfaces.Services;

public interface IMembershipsService
{
    Task<bool> CreateMemberShipBenefitsAsync(int accountId, Membership membership);
    Task RevokeMembershipAsync(int accountId, int membershipId);
    Task ScheduleMembershipExpirationAsync(int accountId, Membership membership);
}
