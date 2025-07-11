using System;
using System.Transactions;
using Gigbuds_BE.Application.DTOs;
using Gigbuds_BE.Application.DTOs.Payments;
using Gigbuds_BE.Application.DTOs.Transactions;
using MediatR;

namespace Gigbuds_BE.Application.Features.Transactions;

public class GetAllTransactionsQuery : IRequest<List<TransactionResponseDto>>
{
}
