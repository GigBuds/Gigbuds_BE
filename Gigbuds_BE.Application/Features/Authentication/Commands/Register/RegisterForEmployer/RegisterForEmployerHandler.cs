using Gigbuds_BE.Application.Interfaces.Services;
using Gigbuds_BE.Domain.Entities.Constants;
using Gigbuds_BE.Domain.Entities.Identity;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gigbuds_BE.Application.Features.Authentication.Commands.Register.RegisterForEmployer
{
    public class RegisterForEmployerHandler(
        IMediator mediator, 
        IApplicationUserService<ApplicationUser> applicationUserService,
        IVerificationCodeService verificationCodeService,
        ISmsService smsService) : IRequestHandler<RegisterForEmployerCommand>
    {
        public async Task Handle(RegisterForEmployerCommand request, CancellationToken cancellationToken)
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
            
            await applicationUserService.InsertEmployerAsync(user, user.Password, request.BusinessEmail);
            
            // Send verification code after successful registration
            try
            {
                var verificationCode = await verificationCodeService.GenerateVerificationCodeAsync(request.PhoneNumber);
                await smsService.SendVerificationCodeAsync(request.PhoneNumber, verificationCode);
            }
            catch (Exception ex)
            {
                throw new BadHttpRequestException(ex.Message);
            }
        }
    }
}
