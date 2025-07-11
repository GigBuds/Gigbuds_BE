using System;
using AutoMapper;
using Gigbuds_BE.Application.DTOs.Transactions;
using Gigbuds_BE.Domain.Entities.Transactions;

namespace Gigbuds_BE.Application.Profiles;

public class TransactionsProfile : Profile
{
    public TransactionsProfile()
    {
        CreateProjection<TransactionRecord, TransactionResponseDto>()
            .ForMember(dest => dest.MembershipName, opt => opt.MapFrom(src => src.Membership.Title));
    }
}
