using System;
using Gigbuds_BE.Application.DTOs;
using Gigbuds_BE.Application.DTOs.ApplicationUsers;
using MediatR;

namespace Gigbuds_BE.Application.Features.Accounts;

public class GetAllAccountQuery : IRequest<List<AccountDto>>
{
    
}
