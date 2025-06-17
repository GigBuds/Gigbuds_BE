using System;

namespace Gigbuds_BE.Application.DTOs.Payments;

public class PaymentWebhookDataDto
{
    public int OrderCode { get; set; }
    public decimal Amount { get; set; }
    public string Status { get; set; } = string.Empty;
} 
