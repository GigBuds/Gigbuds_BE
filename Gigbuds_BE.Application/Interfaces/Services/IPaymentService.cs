using Gigbuds_BE.Application.DTOs.Payments;

namespace Gigbuds_BE.Application.Interfaces.Services;

public interface IPaymentService
{
    /// <summary>
    /// Creates a payment link for membership registration
    /// </summary>
    /// <param name="request">Membership payment request data</param>
    /// <returns>Payment response with checkout URL and QR code</returns>
    Task<MembershipPaymentResponseDto> CreateMembershipPaymentAsync(MembershipPaymentRequestDto request);

    /// <summary>
    /// Creates a general payment link
    /// </summary>
    /// <param name="request">Payment request data</param>
    /// <returns>Payment response with checkout URL and QR code</returns>
    Task<PaymentResponseDto> CreatePaymentLinkAsync(PaymentRequestDto request);

    /// <summary>
    /// Gets payment information by order code or payment link ID
    /// </summary>
    /// <param name="id">Order code or payment link ID</param>
    /// <returns>Payment information</returns>
    Task<PaymentInfoResponseDto> GetPaymentInfoAsync(string id);

    /// <summary>
    /// Cancels a payment link
    /// </summary>
    /// <param name="id">Order code or payment link ID</param>
    /// <param name="cancellationReason">Reason for cancellation</param>
    /// <returns>True if cancellation was successful</returns>
    Task<bool> CancelPaymentAsync(string id, string? cancellationReason = null);

    /// <summary>
    /// Handles payment webhook to update transaction status
    /// </summary>
    /// <param name="webhookData">Webhook data from PayOS</param>
    /// <returns>True if webhook was processed successfully</returns>
    Task<bool> HandlePaymentWebhookAsync(string webhookData);
    Task<bool> HandlePaymentAsync(string status, long orderCode);
} 