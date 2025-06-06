using System;
using Gigbuds_BE.Application.Commons.Constants;
using Gigbuds_BE.Application.Interfaces.Services;
using Gigbuds_BE.Domain.Entities.Constants;
using Gigbuds_BE.Domain.Entities.Identity;
using Gigbuds_BE.Domain.Entities.Memberships;
using Gigbuds_BE.Domain.Entities.Schedule;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Gigbuds_BE.Application.Features.Authentication.Commands.Register.RegisterForUsers;

public class RegisterForUserHandler(
    IMediator mediator,
    IApplicationUserService<ApplicationUser> applicationUserService,
    IVerificationCodeService verificationCodeService,
    ISmsService smsService,
    IMembershipsService membershipsService) : IRequestHandler<RegisterForUserCommand>
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
            JobSeekerSchedule = new JobSeekerSchedule(),
            PhoneNumberConfirmed = false // Set to false initially, will be confirmed via SMS
        };
        
        await applicationUserService.InsertJobSeekerAsync(user, request.Password);
        await applicationUserService.AssignRoleAsync(user, UserRoles.JobSeeker);

        await membershipsService.CreateMemberShipBenefitsAsync(user.Id, new Membership
        {
            Id = 1,
            Title = ProjectConstant.MembershipLevel.Free_Tier_Job_Application_Title,
            MembershipType = MembershipType.JobSeeker,
            Duration = 30,
            Price = 0
        });
        await applicationUserService.UpdateAsync(user);
        
        // Send verification code after successful registration
        try
        {
            var verificationCode = await verificationCodeService.GenerateVerificationCodeAsync(request.PhoneNumber);
            await smsService.SendVerificationCodeAsync(request.PhoneNumber, verificationCode);
        }
        catch (Exception)
        {
            await verificationCodeService.RemoveCodeAsync(request.PhoneNumber);
        }
    }
}
