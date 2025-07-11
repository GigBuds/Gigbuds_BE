using System;
using Gigbuds_BE.Application.DTOs.Transactions;
using Gigbuds_BE.Domain.Entities.Transactions;
using Gigbuds_BE.Application.Interfaces.Repositories;
using MediatR;
using Gigbuds_BE.Application.Specifications.Transactions;
using AutoMapper;

namespace Gigbuds_BE.Application.Features.Transactions;

public class GetAllTransactionsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<GetAllTransactionsQuery, List<TransactionResponseDto>>
{
    public async Task<List<TransactionResponseDto>> Handle(GetAllTransactionsQuery request, CancellationToken cancellationToken)
    {
        var spec = new TransactionRecordSpecification();
        var transactions = await unitOfWork.Repository<TransactionRecord>().GetAllWithSpecificationProjectedAsync<TransactionResponseDto>(spec, mapper.ConfigurationProvider);
        return transactions.ToList();
    }
}   
