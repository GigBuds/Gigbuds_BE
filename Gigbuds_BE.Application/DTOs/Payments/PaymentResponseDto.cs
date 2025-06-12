namespace Gigbuds_BE.Application.DTOs.Payments;

public class PaymentResponseDto
{
    public string Code { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public PaymentDataDto? Data { get; set; }
    public string Signature { get; set; } = string.Empty;
}

public class PaymentDataDto
{
    public string Bin { get; set; } = string.Empty;
    public string AccountNumber { get; set; } = string.Empty;
    public string AccountName { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string Description { get; set; } = string.Empty;
    public int OrderCode { get; set; }
    public string Currency { get; set; } = "VND";
    public string PaymentLinkId { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string CheckoutUrl { get; set; } = string.Empty;
    public string QrCode { get; set; } = string.Empty;
}

public class PaymentInfoResponseDto
{
    public string Code { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public PaymentInfoDataDto? Data { get; set; }
    public string Signature { get; set; } = string.Empty;
}

public class PaymentInfoDataDto
{
    public string Id { get; set; } = string.Empty;
    public int OrderCode { get; set; }
    public decimal Amount { get; set; }
    public decimal AmountPaid { get; set; }
    public decimal AmountRemaining { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public List<TransactionDto> Transactions { get; set; } = new();
    public string? CancellationReason { get; set; }
    public DateTime? CanceledAt { get; set; }
}

public class TransactionDto
{
    public string Reference { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string AccountNumber { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime TransactionDateTime { get; set; }
    public string? VirtualAccountName { get; set; }
    public string? VirtualAccountNumber { get; set; }
    public string? CounterAccountBankId { get; set; }
    public string? CounterAccountBankName { get; set; }
    public string? CounterAccountName { get; set; }
    public string? CounterAccountNumber { get; set; }
}

public class MembershipPaymentRequestDto
{
    public int MembershipId { get; set; }
    public int UserId { get; set; }
    public bool IsMobile { get; set; } = false;
    // public string? BuyerName { get; set; }
    // public string? BuyerEmail { get; set; }
    // public string? BuyerPhone { get; set; }
    // public string? BuyerAddress { get; set; }
}

public class MembershipPaymentResponseDto
{
    public string PaymentLinkId { get; set; } = string.Empty;
    public string CheckoutUrl { get; set; } = string.Empty;
    public string QrCode { get; set; } = string.Empty;
    public long OrderCode { get; set; }
    public decimal Amount { get; set; }
    public string Status { get; set; } = string.Empty;
    public int TransactionId { get; set; }
} 