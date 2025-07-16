using System;
using AutoMapper;
using Gigbuds_BE.Application.DTOs;
using Gigbuds_BE.Domain.Entities.Identity;

namespace Gigbuds_BE.Application.Profiles;

public class AccountProfile : Profile
{
    public AccountProfile()
    {
        CreateMap<ApplicationUser, AccountDto>()
        .ForMember(dest => dest.employerProfileResponseDto, opt => opt.MapFrom(
            src => src.EmployerProfile
        ));
    }
}
