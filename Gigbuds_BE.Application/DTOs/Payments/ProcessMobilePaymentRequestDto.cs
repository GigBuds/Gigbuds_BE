using System;

namespace Gigbuds_BE.Application.DTOs.Payments;

public class ProcessMobilePaymentRequestDto
{
    public string OrderCode { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
} 
