using System;
using Gigbuds_BE.Domain.Entities.Transactions;

namespace Gigbuds_BE.Application.DTOs.Transactions;

public class TransactionResponseDto
{
    public decimal Revenue { get; set; }
    public TransactionStatus TransactionStatus { get; set; }
    public string Content { get; set; }
    public string Gateway { get; set; }
    public long ReferenceCode { get; set; }
    public int? MembershipId { get; set; }
    public string MembershipName { get; set; }
    public int AccountId { get; set; }
}
