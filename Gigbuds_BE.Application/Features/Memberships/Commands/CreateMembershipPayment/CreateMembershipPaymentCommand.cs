using Gigbuds_BE.Application.DTOs.Payments;
using MediatR;

namespace Gigbuds_BE.Application.Features.Memberships.Commands.CreateMembershipPayment;

public class CreateMembershipPaymentCommand : IRequest<MembershipPaymentResponseDto>
{
    public int MembershipId { get; set; }
    public int UserId { get; set; }
    public string? BuyerName { get; set; }
    public string? BuyerEmail { get; set; }
    public string? BuyerPhone { get; set; }
    public string? BuyerAddress { get; set; }
} 