using System;
using Gigbuds_BE.Application.Interfaces.Services;
using Gigbuds_BE.Domain.Entities.Constants;
using Gigbuds_BE.Domain.Entities.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Gigbuds_BE.Application.Features.Authentication.Commands.Register.RegisterForAdmin;

public class RegisterForAdminHandler(
    IMediator mediator,
    IApplicationUserService<ApplicationUser> applicationUserService) : IRequestHandler<RegisterForAdminCommand>
{
    public async Task Handle(RegisterForAdminCommand request, CancellationToken cancellationToken)
    {
        var user = new ApplicationUser
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            UserName = request.PhoneNumber,
            PhoneNumber = request.PhoneNumber,
            Dob = request.Dob,
            Password = request.Password,
            IsMale = request.IsMale,
            PhoneNumberConfirmed = true,
            EmailConfirmed = true,
        };
        
        await applicationUserService.InsertAsync(user, request.Password);
        await applicationUserService.AssignRoleAsync(user, UserRoles.Admin);
    }
} 