using System;
using Gigbuds_BE.Application.Interfaces;
using Gigbuds_BE.Application.Interfaces.Services;
using Gigbuds_BE.Domain.Entities.Identity;
using MediatR;
using Gigbuds_BE.Domain.Exceptions;
using Gigbuds_BE.Application.Interfaces.Repositories;

namespace Gigbuds_BE.Application.Features.Accounts.JobSeekers;

public class BanAccountCommandHandler(IUnitOfWork unitOfWork, IApplicationUserService<ApplicationUser> applicationUserService) : IRequestHandler<BanAccountCommand, bool>
{
    public async Task<bool> Handle(BanAccountCommand request, CancellationToken cancellationToken)
    {
        var account = await applicationUserService.GetByIdAsync(request.Id);
        if (account == null) throw new NotFoundException("Account not found");
        account.IsEnabled = request.IsEnabled;
        await applicationUserService.UpdateAsync(account);
        await unitOfWork.CompleteAsync();
        return true;
    }
}
