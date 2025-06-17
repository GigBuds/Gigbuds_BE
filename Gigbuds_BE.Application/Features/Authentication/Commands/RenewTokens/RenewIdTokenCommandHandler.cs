using System;
using Gigbuds_BE.Application.Interfaces.Services.AuthenticationServices;
using Gigbuds_BE.Domain.Entities.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Gigbuds_BE.Application.Features.Authentication.Commands.RenewTokens;

public class RenewIdTokenCommandHandler(UserManager<ApplicationUser> userManager, IUserTokenService userTokenService) : IRequestHandler<RenewIdTokenCommand, string>
{
    public async Task<string> Handle(RenewIdTokenCommand request, CancellationToken cancellationToken)
    {
        var user = await userManager.Users.Include(u => u.AccountMemberships).ThenInclude(am => am.Membership).FirstOrDefaultAsync(u => u.Id == request.userId);
        if(user == null) {
            throw new Exception("User not found");
        }
        var userRoles = await userManager.GetRolesAsync(user);
        var newIdToken = userTokenService.CreateIdToken(user, userRoles.ToList());
        return newIdToken;
    }
}
