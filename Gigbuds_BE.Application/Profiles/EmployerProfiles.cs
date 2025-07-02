using System;
using AutoMapper;
using Gigbuds_BE.Application.DTOs.ApplicationUsers;
using Gigbuds_BE.Domain.Entities.Accounts;
using Gigbuds_BE.Domain.Entities.Feedbacks;
using Gigbuds_BE.Domain.Entities.Identity;

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

        CreateMap<ApplicationUser, MyEmployerProfileResponseDto>()
            .ForMember(dest => dest.CompanyEmail, opt => opt.MapFrom(src => src.EmployerProfile.CompanyEmail))
            .ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => src.EmployerProfile.CompanyName))
            .ForMember(dest => dest.CompanyAddress, opt => opt.MapFrom(src => src.EmployerProfile.CompanyAddress))
            .ForMember(dest => dest.TaxNumber, opt => opt.MapFrom(src => src.EmployerProfile.TaxNumber))
            .ForMember(dest => dest.BusinessLicense, opt => opt.MapFrom(src => src.EmployerProfile.BusinessLicense))
            .ForMember(dest => dest.CompanyLogo, opt => opt.MapFrom(src => src.EmployerProfile.CompanyLogo))
            .ForMember(dest => dest.NumOfAvailablePost, opt => opt.MapFrom(src => src.EmployerProfile.NumOfAvailablePost))
            .ForMember(dest => dest.AverageRating, opt => opt.MapFrom(src => src.EmployerFeedbacks.Count > 0 ? src.EmployerFeedbacks.Average(f => f.Rating) : 0))
            .ForMember(dest => dest.NumOfFollowers, opt => opt.MapFrom(src => src.Followers.Count))
            .ForMember(dest => dest.CompanyDescription, opt => opt.MapFrom(src => src.EmployerProfile.CompanyDescription));
    }
}
