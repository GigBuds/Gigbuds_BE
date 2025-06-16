using System;

namespace Gigbuds_BE.Application.DTOs.Payments;

public class CancelPaymentRequestDto
{
    public string? CancellationReason { get; set; }
}
