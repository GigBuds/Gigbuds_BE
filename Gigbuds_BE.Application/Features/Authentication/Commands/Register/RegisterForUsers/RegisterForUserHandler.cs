using System;
using Gigbuds_BE.Application.Interfaces.Services;
using Gigbuds_BE.Domain.Entities.Constants;
using Gigbuds_BE.Domain.Entities.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Gigbuds_BE.Application.Features.Authentication.Commands.Register.RegisterForUsers;

public class RegisterForUserHandler(
    IMediator mediator,
    IApplicationUserService<ApplicationUser> applicationUserService,
    IVerificationCodeService verificationCodeService,
    ISmsService smsService) : IRequestHandler<RegisterForUserCommand>
{
    public async Task Handle(RegisterForUserCommand request, CancellationToken cancellationToken)
    {
        var user = new ApplicationUser
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            UserName = request.PhoneNumber,
            PhoneNumber = request.PhoneNumber,
            Dob = request.Dob,
            IsMale = request.IsMale,
            SocialSecurityNumber = request.SocialSecurityNumber,
            Password = request.Password,
            PhoneNumberConfirmed = false // Set to false initially, will be confirmed via SMS
        };
        
        await applicationUserService.InsertJobSeekerAsync(user, request.Password);
        await applicationUserService.AssignRoleAsync(user, UserRoles.JobSeeker);
        
        // Send verification code after successful registration
        try
        {
            var verificationCode = await verificationCodeService.GenerateVerificationCodeAsync(request.PhoneNumber);
            await smsService.SendVerificationCodeAsync(request.PhoneNumber, verificationCode);
        }
        catch (Exception)
        {
            // Log error but don't fail registration if SMS fails
            // The user can request a new verification code later
        }
    }
}
