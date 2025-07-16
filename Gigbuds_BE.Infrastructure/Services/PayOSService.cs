using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Gigbuds_BE.Application.Configurations;
using Gigbuds_BE.Application.DTOs.Payments;
using Gigbuds_BE.Application.Interfaces.Repositories;
using Gigbuds_BE.Application.Interfaces.Services;
using Gigbuds_BE.Application.Specifications.Memberships;
using Gigbuds_BE.Application.Specifications.Transactions;
using Gigbuds_BE.Domain.Entities.Memberships;
using Gigbuds_BE.Domain.Entities.Transactions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Net.payOS;
using Net.payOS.Types;

namespace Gigbuds_BE.Infrastructure.Services;

public class PayOSService : IPaymentService
{
    private readonly PayOSSettings _settings;
    private readonly ILogger<PayOSService> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMembershipsService _membershipsService;
    private readonly PayOS _payOS;

    public PayOSService(
        IOptions<PayOSSettings> settings, 
        ILogger<PayOSService> logger,
        IUnitOfWork unitOfWork,
        IMembershipsService membershipsService)
    {
        _settings = settings.Value;
        _logger = logger;
        _unitOfWork = unitOfWork;
        _membershipsService = membershipsService;

        // Initialize PayOS SDK
        _payOS = new PayOS(_settings.ClientId, _settings.ApiKey, _settings.ChecksumKey);
    }

    public async Task<MembershipPaymentResponseDto> CreateMembershipPaymentAsync(MembershipPaymentRequestDto request)
    {
        try
        {
            // Get membership details
            var spec = new GetMembershipByIdSpecification(request.MembershipId);
            var membership = await _unitOfWork.Repository<Membership>().GetBySpecificationAsync(spec);
            
            if (membership == null)
            {
                throw new ArgumentException("Membership not found");
            }

            // Generate unique order code
            var orderCode = GenerateOrderCode();

            // Create transaction record first
            var transaction = new TransactionRecord
            {
                Revenue = membership.Price,
                TransactionStatus = TransactionStatus.Pending,
                Content = $"Membership payment for {membership.Title}",
                Gateway = "PayOS",
                ReferenceCode = orderCode,
                MembershipId = membership.Id,
                AccountId = request.UserId
            };

            _unitOfWork.Repository<TransactionRecord>().Insert(transaction);
            await _unitOfWork.CompleteAsync();

            // Create payment data using PayOS SDK types
            var items = new List<ItemData>
            {
                new ItemData(membership.Title, 1, (int)membership.Price)
            };

            var paymentData = new PaymentData(
                orderCode: orderCode,
                amount: (int)membership.Price,
                description: $"{membership.Title}",
                items: items,
                cancelUrl: request.IsMobile ? _settings.MobileCancelUrl : _settings.CancelUrl,
                returnUrl: request.IsMobile ? _settings.MobileReturnUrl : _settings.ReturnUrl,
                expiredAt: DateTimeOffset.UtcNow.AddMinutes(_settings.ExpirationMinutes).ToUnixTimeSeconds()
            );

            // Create payment link using PayOS SDK
            var createPaymentResult = await _payOS.createPaymentLink(paymentData);

            if (createPaymentResult == null)
            {
                // Update transaction as failed
                transaction.TransactionStatus = TransactionStatus.Failed;
                _unitOfWork.Repository<TransactionRecord>().Update(transaction);
                await _unitOfWork.CompleteAsync();

                throw new Exception("Failed to create payment link");
            }

            _logger.LogInformation("Successfully created PayOS payment link for order {OrderCode}", orderCode);

            return new MembershipPaymentResponseDto
            {
                PaymentLinkId = createPaymentResult.paymentLinkId,
                CheckoutUrl = createPaymentResult.checkoutUrl,
                QrCode = createPaymentResult.qrCode,
                OrderCode = createPaymentResult.orderCode,
                Amount = createPaymentResult.amount,
                Status = createPaymentResult.status,
                TransactionId = transaction.Id
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating membership payment for user {UserId}, membership {MembershipId}", 
                request.UserId, request.MembershipId);
            throw;
        }
    }

    public async Task<PaymentResponseDto> CreatePaymentLinkAsync(PaymentRequestDto request)
    {
        try
        {
            // Convert request items to PayOS SDK format
            var items = request.Items.Select(i => new ItemData(i.Name, i.Quantity, (int)i.Price)).ToList();

            // Determine return URLs based on mobile flag
            string returnUrl;
            string cancelUrl;
            
            if (request.IsMobile)
            {
                // For mobile, use intermediate page URLs with orderCode parameter
                var webDomain = "https://gigbuds-web.vercel.app"; // Configure this in appsettings.json
                returnUrl = request.ReturnUrl ?? $"{webDomain}/payment/mobile-intermediate?orderCode={request.OrderCode}&status=success";
                cancelUrl = request.CancelUrl ?? $"{webDomain}/payment/mobile-intermediate?orderCode={request.OrderCode}&status=cancelled";
            }
            else
            {
                // For web, use the configured URLs
                returnUrl = request.ReturnUrl ?? _settings.ReturnUrl;
                cancelUrl = request.CancelUrl ?? _settings.CancelUrl;
            }

            var paymentData = new PaymentData(
                orderCode: request.OrderCode,
                amount: (int)request.Amount,
                description: request.Description,
                items: items,
                cancelUrl: cancelUrl,
                returnUrl: returnUrl,
                expiredAt: request.ExpiredAt,
                buyerName: request.BuyerName,
                buyerEmail: request.BuyerEmail,
                buyerPhone: request.BuyerPhone,
                buyerAddress: request.BuyerAddress
            );

            var createPaymentResult = await _payOS.createPaymentLink(paymentData);

            _logger.LogInformation("Creating PayOS payment link for order {OrderCode} (Mobile: {IsMobile})", request.OrderCode, request.IsMobile);

            // Convert PayOS SDK result to our DTO format
            return new PaymentResponseDto
            {
                Code = "00", // Success code
                Description = "Success",
                Data = createPaymentResult != null ? new PaymentDataDto
                {
                    PaymentLinkId = createPaymentResult.paymentLinkId,
                    CheckoutUrl = createPaymentResult.checkoutUrl,
                    QrCode = createPaymentResult.qrCode,
                    OrderCode = (int)createPaymentResult.orderCode,
                    Amount = createPaymentResult.amount,
                    Status = createPaymentResult.status,
                    Currency = "VND"
                } : null
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating payment link for order {OrderCode}", request.OrderCode);
            throw;
        }
    }

    public async Task<PaymentInfoResponseDto> GetPaymentInfoAsync(string id)
    {
        try
        {
            // Parse order code (id can be orderCode or paymentLinkId)
            if (!long.TryParse(id, out var orderCode))
            {
                throw new ArgumentException("Invalid order code format");
            }

            var paymentInfo = await _payOS.getPaymentLinkInformation(orderCode);

            if (paymentInfo == null)
            {
                throw new Exception($"Payment information not found for order {id}");
            }

            _logger.LogInformation("Successfully retrieved payment info for order {OrderCode}", orderCode);

            // Convert PayOS SDK result to our DTO format
            return new PaymentInfoResponseDto
            {
                Code = "00",
                Description = "Success",
                Data = new PaymentInfoDataDto
                {
                    Id = paymentInfo.id,
                    OrderCode = (int)paymentInfo.orderCode,
                    Amount = paymentInfo.amount,
                    AmountPaid = paymentInfo.amountPaid,
                    AmountRemaining = paymentInfo.amountRemaining,
                    Status = paymentInfo.status,
                    CreatedAt = DateTime.Parse(paymentInfo.createdAt),
                    CancellationReason = paymentInfo.cancellationReason,
                    CanceledAt = paymentInfo.canceledAt != null ? DateTime.Parse(paymentInfo.canceledAt) : null,
                    Transactions = paymentInfo.transactions.Select(t => new TransactionDto
                    {
                        Reference = t.reference,
                        Amount = t.amount,
                        AccountNumber = t.accountNumber,
                        Description = t.description,
                        TransactionDateTime = DateTime.Parse(t.transactionDateTime),
                        VirtualAccountName = t.virtualAccountName,
                        VirtualAccountNumber = t.virtualAccountNumber,
                        CounterAccountBankId = t.counterAccountBankId,
                        CounterAccountBankName = t.counterAccountBankName,
                        CounterAccountName = t.counterAccountName,
                        CounterAccountNumber = t.counterAccountNumber
                    }).ToList()
                }
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting payment info for {Id}", id);
            throw;
        }
    }

    public async Task<bool> CancelPaymentAsync(string id, string? cancellationReason = null)
    {
        try
        {
            // Parse order code
            if (!long.TryParse(id, out var orderCode))
            {
                _logger.LogError("Invalid order code format: {Id}", id);
                return false;
            }

            var cancelledPayment = await _payOS.cancelPaymentLink(orderCode, cancellationReason ?? "User cancelled");

            if (cancelledPayment != null)
            {
                _logger.LogInformation("Successfully cancelled payment {Id}", id);
                return true;
            }

            _logger.LogError("Failed to cancel payment {Id}", id);
            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error cancelling payment {Id}", id);
            return false;
        }
    }

    public async Task<bool> HandlePaymentWebhookAsync(string webhookData)
    {
        try
        {
            // Parse webhook data
            var webhookType = JsonSerializer.Deserialize<WebhookType>(webhookData, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (webhookType == null)
            {
                _logger.LogWarning("Invalid webhook payload received");
                return false;
            }

            // Verify webhook data using PayOS SDK
            var verifiedWebhookData = _payOS.verifyPaymentWebhookData(webhookType);

        if (verifiedWebhookData == null)
            {
                _logger.LogWarning("Failed to verify webhook data");
                return false;
            }

            // Find transaction by order code
            var transactions = await _unitOfWork.Repository<TransactionRecord>()
                .GetAllWithSpecificationAsync(new TransactionByReferenceCodeSpecification(verifiedWebhookData.orderCode));

            var transaction = transactions.FirstOrDefault();
            if (transaction == null)
            {
                _logger.LogWarning("Transaction not found for order code {OrderCode}", verifiedWebhookData.orderCode);
                return false;
            }

            var paymentStatus = verifiedWebhookData.code switch
            {
                "00" => TransactionStatus.Completed, // PayOS success code
                "01" => TransactionStatus.Failed,    // PayOS failure code
                _ => TransactionStatus.Pending
            };
            
            // Update transaction status based on webhook data
            transaction.TransactionStatus = verifiedWebhookData.code switch
            {
                "00" => TransactionStatus.Completed, // PayOS success code
                "01" => TransactionStatus.Failed,    // PayOS failure code
                _ => TransactionStatus.Pending
            };

            _unitOfWork.Repository<TransactionRecord>().Update(transaction);
            await _unitOfWork.CompleteAsync();

            // If payment is successful and it's a membership payment, activate the membership
            if (transaction.TransactionStatus == TransactionStatus.Completed && transaction.MembershipId.HasValue)
            {
                await _membershipsService.ProcessMembershipPaymentSuccessAsync((int)verifiedWebhookData.orderCode);
            }

            _logger.LogInformation("Successfully processed webhook for order code {OrderCode}, status: {Status}", 
                verifiedWebhookData.orderCode, verifiedWebhookData.desc);

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing payment webhook");
            return false;
        }
    }

    private long GenerateOrderCode()
    {
        // Generate a unique order code using timestamp and random number
        var timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        var random = new Random().Next(1000, 9999);
        // Ensure the result is positive and fits within long range
        var orderCode = (timestamp % 100000) * 10000 + random;
        return Math.Abs(orderCode);
    }

    public async Task<bool> HandlePaymentAsync(string status, long orderCode)
    {
        var transaction = await _unitOfWork.Repository<TransactionRecord>().GetBySpecificationAsync(new TransactionByReferenceCodeSpecification(orderCode));
        if (transaction == null)
        {
            _logger.LogWarning("Transaction not found for order code {OrderCode}", orderCode);
            return false;
        }
        transaction.TransactionStatus = status switch
        {
            "PAID" => TransactionStatus.Completed,
            "FAILED" => TransactionStatus.Failed,
            "CANCELLED" => TransactionStatus.Cancelled,
            _ => transaction.TransactionStatus
        };

        _unitOfWork.Repository<TransactionRecord>().Update(transaction);
            await _unitOfWork.CompleteAsync();

        if (transaction.TransactionStatus == TransactionStatus.Completed && transaction.MembershipId.HasValue)
        {
            await _membershipsService.ProcessMembershipPaymentSuccessAsync(transaction.ReferenceCode);
        }

        return true;
    }
}

