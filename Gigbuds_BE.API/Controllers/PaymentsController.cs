using Gigbuds_BE.Application.DTOs.Payments;
using Gigbuds_BE.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Gigbuds_BE.API.Controllers;

public class PaymentsController : _BaseApiController
{
    private readonly IPaymentService _paymentService;
    private readonly ILogger<PaymentsController> _logger;

    public PaymentsController(IPaymentService paymentService, ILogger<PaymentsController> logger)
    {
        _paymentService = paymentService;
        _logger = logger;
    }

    /// <summary>
    /// Creates a payment link for membership registration
    /// </summary>
    /// <param name="request">Membership payment request</param>
    /// <returns>Payment response with checkout URL and QR code</returns>
    [HttpPost("memberships")]
    public async Task<IActionResult> CreateMembershipPayment([FromBody] MembershipPaymentRequestDto request)
    {
        try
        {
            var result = await _paymentService.CreateMembershipPaymentAsync(request);
            return Ok(new
            {
                success = true,
                data = result,
                message = "Payment link created successfully"
            });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { success = false, error = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating membership payment for user {UserId}, membership {MembershipId}", 
                request.UserId, request.MembershipId);
            return StatusCode(500, new { success = false, error = "Internal server error" });
        }
    }

    /// <summary>
    /// Creates a general payment link
    /// </summary>
    /// <param name="request">Payment request</param>
    /// <returns>Payment response with checkout URL and QR code</returns>
    [HttpPost("create")]
    [Authorize]
    public async Task<IActionResult> CreatePaymentLink([FromBody] PaymentRequestDto request)
    {
        try
        {
            var result = await _paymentService.CreatePaymentLinkAsync(request);
            return Ok(new
            {
                success = true,
                data = result,
                message = "Payment link created successfully"
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating payment link for order {OrderCode}", request.OrderCode);
            return StatusCode(500, new { success = false, error = "Internal server error" });
        }
    }

    /// <summary>
    /// Gets payment information by order code or payment link ID
    /// </summary>
    /// <param name="id">Order code or payment link ID</param>
    /// <returns>Payment information</returns>
    [HttpGet("{id}")]
    [Authorize]
    public async Task<IActionResult> GetPaymentInfo(string id)
    {
        try
        {
            var result = await _paymentService.GetPaymentInfoAsync(id);
            return Ok(new
            {
                success = true,
                data = result,
                message = "Payment information retrieved successfully"
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting payment info for {Id}", id);
            return StatusCode(500, new { success = false, error = "Internal server error" });
        }
    }

    /// <summary>
    /// Cancels a payment link
    /// </summary>
    /// <param name="id">Order code or payment link ID</param>
    /// <param name="request">Cancellation request</param>
    /// <returns>Cancellation result</returns>
    [HttpPost("{id}/cancel")]
    [Authorize]
    public async Task<IActionResult> CancelPayment(string id, [FromBody] CancelPaymentRequestDto request)
    {
        try
        {
            var result = await _paymentService.CancelPaymentAsync(id, request.CancellationReason);
            if (result)
            {
                return Ok(new
                {
                    success = true,
                    message = "Payment cancelled successfully"
                });
            }

            return BadRequest(new { success = false, error = "Failed to cancel payment" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error cancelling payment {Id}", id);
            return StatusCode(500, new { success = false, error = "Internal server error" });
        }
    }

    /// <summary>
    /// Webhook endpoint for PayOS payment notifications
    /// Works for both web and mobile applications
    /// </summary>
    /// <returns>Webhook processing result</returns>
    [HttpPost("webhook")]
    [AllowAnonymous]
    public async Task<IActionResult> PaymentWebhook()
    {
        try
        {
            using var reader = new StreamReader(Request.Body);
            var webhookData = await reader.ReadToEndAsync();

            var result = await _paymentService.HandlePaymentWebhookAsync(webhookData);
            
            if (result)
            {
                return Ok(new { success = true, message = "Webhook processed successfully" });
            }

            return BadRequest(new { success = false, error = "Failed to process webhook" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing payment webhook");
            return StatusCode(500, new { success = false, error = "Internal server error" });
        }
    }

    /// <summary>
    /// Return URL endpoint for successful payments (used for web and mobile deep links)
    /// </summary>
    /// <param name="orderCode">Order code from PayOS</param>
    /// <param name="status">Payment status</param>
    /// <returns>Payment result page or redirect</returns>
    [HttpGet("return")]
    [AllowAnonymous]
    public async Task<IActionResult> PaymentReturn([FromQuery] string orderCode, [FromQuery] string status)
    {
        try
        {
            _logger.LogInformation("Payment return for order {OrderCode} with status {Status}", orderCode, status);
            
            // Parse orderCode to long for service call
            if (!long.TryParse(orderCode, out long orderCodeLong))
            {
                return BadRequest(new { 
                    success = false, 
                    message = "Invalid order code format",
                    orderCode = orderCode,
                    status = "ERROR"
                });
            }
            
            var result = await _paymentService.HandlePaymentAsync(status, orderCodeLong);
            
            if (status?.ToUpper() == "PAID")
            {
                return Ok(new { 
                    success = true, 
                    message = "Payment completed successfully! Your membership has been activated.",
                    orderCode = orderCode,
                    status = "SUCCESS"
                });
            }
            else if (status?.ToUpper() == "CANCELLED")
            {
                return Ok(new { 
                    success = false, 
                    message = "Payment was cancelled. No charges were made to your account.",
                    orderCode = orderCode,
                    status = "CANCELLED"
                });
            }
            else
            {
                return Ok(new { 
                    success = false, 
                    message = "Payment failed. Please try again or contact support.",
                    orderCode = orderCode,
                    status = "FAILED"
                });
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing payment return for order {OrderCode}", orderCode);
            return Ok(new { 
                success = false, 
                message = "An error occurred while processing your payment. Please contact support.",
                orderCode = orderCode,
                status = "ERROR"
            });
        }
    }

    /// <summary>
    /// Process mobile payment result - called by frontend intermediate page
    /// Registers membership and updates transaction status before mobile app redirect
    /// </summary>
    /// <param name="request">Mobile payment processing request</param>
    /// <returns>Processing result</returns>
    [HttpPost("process-mobile-payment")]
    [AllowAnonymous]
    public async Task<IActionResult> ProcessMobilePayment([FromBody] ProcessMobilePaymentRequestDto request)
    {
        try
        {
            _logger.LogInformation("Processing mobile payment for order {OrderCode} with status {Status}", 
                request.OrderCode, request.Status);
            
            // Parse orderCode to long for service call
            if (!long.TryParse(request.OrderCode, out long orderCodeLong))
            {
                return BadRequest(new { 
                    success = false, 
                    message = "Invalid order code format"
                });
            }
            
            // Process the payment using existing service method
            var result = await _paymentService.HandlePaymentAsync(request.Status, orderCodeLong);
            
            if (request.Status?.ToUpper() == "PAID")
            {
                return Ok(new { 
                    success = true, 
                    message = "Payment processed successfully! Your membership has been activated.",
                    orderCode = request.OrderCode,
                    status = "SUCCESS"
                });
            }
            else if (request.Status?.ToUpper() == "CANCELLED")
            {
                return Ok(new { 
                    success = true, 
                    message = "Payment was cancelled. No charges were made to your account.",
                    orderCode = request.OrderCode,
                    status = "CANCELLED"
                });
            }
            else
            {
                return BadRequest(new { 
                    success = false, 
                    message = "Payment failed. Please try again or contact support.",
                    orderCode = request.OrderCode,
                    status = "FAILED"
                });
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing mobile payment for order {OrderCode}", request.OrderCode);
            return StatusCode(500, new { 
                success = false, 
                message = "An error occurred while processing your payment. Please contact support."
            });
        }
    }

    /// <summary>
    /// Cancel URL endpoint for cancelled payments
    /// </summary>
    /// <param name="orderCode">Order code from PayOS</param>
    /// <returns>Payment cancelled page</returns>
    [HttpGet("cancel")]
    [AllowAnonymous]
    public IActionResult PaymentCancel([FromQuery] int orderCode)
    {
        try
        {
            _logger.LogInformation("Payment cancelled for order {OrderCode}", orderCode);
            
            return Redirect($"/payment/cancelled?orderCode={orderCode}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error handling payment cancellation for order {OrderCode}", orderCode);
            return Redirect($"/payment/cancelled?orderCode={orderCode}");
        }
    }
}