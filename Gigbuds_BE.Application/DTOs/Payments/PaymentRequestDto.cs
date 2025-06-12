using System.ComponentModel.DataAnnotations;

namespace Gigbuds_BE.Application.DTOs.Payments;

public class PaymentRequestDto
{
    [Required]
    public int OrderCode { get; set; }

    [Required]
    [Range(1000, double.MaxValue, ErrorMessage = "Amount must be at least 1000 VND")]
    public decimal Amount { get; set; }

    [Required]
    [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
    public string Description { get; set; } = string.Empty;

    public string? BuyerName { get; set; }
    public string? BuyerEmail { get; set; }
    public string? BuyerPhone { get; set; }
    public string? BuyerAddress { get; set; }

    public List<PaymentItemDto> Items { get; set; } = new();

    public string? CancelUrl { get; set; }
    public string? ReturnUrl { get; set; }
    public long? ExpiredAt { get; set; }
    public string? Signature { get; set; }
    
    /// <summary>
    /// Flag to determine if this is a mobile payment request
    /// </summary>
    public bool IsMobile { get; set; } = false;
}

public class PaymentItemDto
{
    [Required]
    public string Name { get; set; } = string.Empty;

    [Required]
    [Range(1, int.MaxValue)]
    public int Quantity { get; set; }

    [Required]
    [Range(1000, double.MaxValue)]
    public decimal Price { get; set; }
} 