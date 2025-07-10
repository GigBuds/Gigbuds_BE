using System;
using AutoMapper;
using Gigbuds_BE.Application.DTOs;
using Gigbuds_BE.Application.Interfaces;
using Gigbuds_BE.Application.Interfaces.Repositories;
using Gigbuds_BE.Application.Interfaces.Services;
using Gigbuds_BE.Application.Specifications.ApplicationUsers;
using Gigbuds_BE.Domain.Entities.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;

namespace Gigbuds_BE.Application.Features.Accounts;
    
public class GetAllAccountQueryHandler(IUnitOfWork unitOfWork, IApplicationUserService<ApplicationUser> applicationUserService, IMapper mapper, UserManager<ApplicationUser> userManager) : IRequestHandler<GetAllAccountQuery, List<AccountDto>>
{

    public async Task<List<AccountDto>> Handle(GetAllAccountQuery request, CancellationToken cancellationToken)
    {
        var spec = new GetAllAccountSpec();
        var accounts = await applicationUserService.GetAllUsersWithSpecAsync(spec);
        var accountDtos = new List<AccountDto>();
        foreach (var account in accounts)
        {
            var dto = mapper.Map<AccountDto>(account);
            var roles = await userManager.GetRolesAsync(account);
            dto.Roles = roles.ToList();
            accountDtos.Add(dto);
        }
        return accountDtos;
    }
}
