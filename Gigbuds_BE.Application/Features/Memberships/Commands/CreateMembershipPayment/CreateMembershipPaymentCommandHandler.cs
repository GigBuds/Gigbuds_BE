using Gigbuds_BE.Application.DTOs.Payments;
using Gigbuds_BE.Application.Interfaces.Services;
using MediatR;

namespace Gigbuds_BE.Application.Features.Memberships.Commands.CreateMembershipPayment;

public class CreateMembershipPaymentCommandHandler : IRequestHandler<CreateMembershipPaymentCommand, MembershipPaymentResponseDto>
{
    private readonly IPaymentService _paymentService;

    public CreateMembershipPaymentCommandHandler(IPaymentService paymentService)
    {
        _paymentService = paymentService;
    }

    public async Task<MembershipPaymentResponseDto> Handle(CreateMembershipPaymentCommand request, CancellationToken cancellationToken)
    {
        var paymentRequest = new MembershipPaymentRequestDto
        {
            MembershipId = request.MembershipId,
            UserId = request.UserId,
            //BuyerName = request.BuyerName,
            //BuyerEmail = request.BuyerEmail,
            //BuyerPhone = request.BuyerPhone,
            //BuyerAddress = request.BuyerAddress
        };

        return await _paymentService.CreateMembershipPaymentAsync(paymentRequest);
    }
} 