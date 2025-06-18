using System;
using AutoMapper;
using Gigbuds_BE.Application.DTOs.ApplicationUsers;
using Gigbuds_BE.Application.DTOs.Files;
using Gigbuds_BE.Application.Interfaces.Repositories;
using Gigbuds_BE.Application.Interfaces.Services;
using Gigbuds_BE.Application.Specifications.EmployerProfiles;
using Gigbuds_BE.Domain.Entities.Accounts;
using Gigbuds_BE.Domain.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Gigbuds_BE.Application.Features.Accounts.EmployerProfiles.Commands;

public class UpdateEmployerProfileCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IFileStorageService fileStorageService) : IRequestHandler<UpdateEmployerProfileCommand, EmployerProfileResponseDto>
{
    public async Task<EmployerProfileResponseDto> Handle(UpdateEmployerProfileCommand request, CancellationToken cancellationToken)
    {
        var spec = new GetEmployerProfleByAccountIdSpecification(request.AccountId);
        var employerProfile = await unitOfWork.Repository<EmployerProfile>().GetBySpecificationAsync(spec);

        if (employerProfile == null)
        {
            throw new NotFoundException($"Employer profile with id {request.AccountId} not found");
        }
        employerProfile.CompanyEmail = request.CompanyEmail;
        employerProfile.CompanyName = request.CompanyName;
        employerProfile.CompanyAddress = request.CompanyAddress;
        employerProfile.TaxNumber = request.TaxNumber;

        var companyLogoUrl = await UploadCompanyLogo(request.CompanyLogo, employerProfile);
        employerProfile.CompanyLogo = companyLogoUrl;

        var businessLicenseUrl = await UploadBusinessLicense(request.BusinessLicense, employerProfile);
        employerProfile.BusinessLicense = businessLicenseUrl;

        unitOfWork.Repository<EmployerProfile>().Update(employerProfile);
        await unitOfWork.CompleteAsync();

        return mapper.Map<EmployerProfileResponseDto>(employerProfile); 
    }

    private async Task<string?> UploadBusinessLicense(IFormFile? businessLicense, EmployerProfile employerProfile) {
        if(businessLicense != null && businessLicense.Length > 0) {
            try {

                if(employerProfile.BusinessLicense != null) {
                    await fileStorageService.DeleteFileAsync(employerProfile.BusinessLicense);
                }

                FileUploadResult uploadResult = await fileStorageService.PrepareUploadFileAsync(businessLicense, "business-licenses", FileType.Document);
                
                if(uploadResult.Success) {
                    return uploadResult.FileUrl;
                }

                return null;

            } catch (Exception ex) {
                throw new Exception("Failed to upload business license: " + ex.Message);
            }
        }

        return employerProfile.BusinessLicense;
    }

    private async Task<string?> UploadCompanyLogo(IFormFile? companyLogo, EmployerProfile employerProfile) {
        if(companyLogo != null && companyLogo.Length > 0) {
            try {
                
                if(employerProfile.CompanyLogo != null) {
                    await fileStorageService.DeleteFileAsync(employerProfile.CompanyLogo);
                }

                FileUploadResult uploadResult = await fileStorageService.PrepareUploadImageAsync(companyLogo, "company-logos");

                if(uploadResult.Success) {
                    return uploadResult.FileUrl;
                } else {
                    throw new Exception(uploadResult.ErrorMessage);
                }

            } catch (Exception ex) {
                throw new Exception("Failed to upload company logo: " + ex.Message);
            }
            
        }

        return employerProfile.CompanyLogo;
    }
}
