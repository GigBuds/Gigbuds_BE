using System;
using Gigbuds_BE.Domain.Entities.Memberships;

namespace Gigbuds_BE.Application.DTOs.Memberships;

public class MemberShipByIdDto
{
    public int MembershipId { get; set; }
    public int AccountId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public AccountMembershipStatus Status { get; set; }
    public MembershipType MembershipType { get; set; }
    public string Title { get; set; }
}
