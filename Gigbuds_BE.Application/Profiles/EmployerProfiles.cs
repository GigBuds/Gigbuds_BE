using System;
using AutoMapper;
using Gigbuds_BE.Application.DTOs.ApplicationUsers;
using Gigbuds_BE.Domain.Entities.Accounts;

namespace Gigbuds_BE.Application.Profiles;

public class EmployerProfileProfiles : Profile
{
    public EmployerProfileProfiles()
    {
        CreateMap<EmployerProfile, EmployerProfileResponseDto>()
            .ForMember(dest => dest.CompanyEmail, opt => opt.MapFrom(src => src.CompanyEmail))
            .ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => src.CompanyName))
            .ForMember(dest => dest.CompanyAddress, opt => opt.MapFrom(src => src.CompanyAddress))
            .ForMember(dest => dest.TaxNumber, opt => opt.MapFrom(src => src.TaxNumber))
            .ForMember(dest => dest.BusinessLicense, opt => opt.MapFrom(src => src.BusinessLicense))
            .ForMember(dest => dest.CompanyLogo, opt => opt.MapFrom(src => src.CompanyLogo));
    }
}
