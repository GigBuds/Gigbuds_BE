using System;
using Gigbuds_BE.Domain.Entities.Memberships;

namespace Gigbuds_BE.Application.DTOs.Memberships;

public class MembershipResponseDto
{
    public int UserId { get; set; }
    public MembershipType MembershipType { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public decimal Price { get; set; }
}
