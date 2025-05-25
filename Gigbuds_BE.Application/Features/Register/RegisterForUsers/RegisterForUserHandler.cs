using System;
using Gigbuds_BE.Application.Interfaces.Services;
using Gigbuds_BE.Domain.Entities.Constants;
using Gigbuds_BE.Domain.Entities.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Gigbuds_BE.Application.Features.Register.RegisterForUsers;

public class RegisterForUserHandler(
    IMediator mediator,
    IApplicationUserService<ApplicationUser> applicationUserService) : IRequestHandler<RegisterForUserCommand>
{
    public async Task Handle(RegisterForUserCommand request, CancellationToken cancellationToken)
    {
        var user = new ApplicationUser
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            PhoneNumber = request.PhoneNumber,
            Dob = request.Dob,
            IsMale = request.IsMale,
            SocialSecurityNumber = request.SocialSecurityNumber,
            Password = request.Password,
        };
        await applicationUserService.InsertAsync(user, request.Password);
        await applicationUserService.AssignRoleAsync(user, UserRoles.JobSeeker);
    }
}
