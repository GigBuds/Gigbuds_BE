using System;
using Gigbuds_BE.Domain.Entities.Memberships;

namespace Gigbuds_BE.Application.DTOs.Memberships;

public class MembershipDetailResponseDto
{
    public string Title { get; set; }
    public MembershipType MembershipType { get; set; }
    public int Duration { get; set; }
    public decimal Price { get; set; }
    public string Description { get; set; }
}
