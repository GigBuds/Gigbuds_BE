using System;
using AutoMapper;
using Gigbuds_BE.Application.DTOs.Followers;
using Gigbuds_BE.Domain.Entities.Accounts;

namespace Gigbuds_BE.Application.Profiles;

public class FollowerProfiles : Profile
{
    public FollowerProfiles()
    {
        CreateMap<Follower, FollowerResponseDto>()
            .ForMember(dest => dest.FollowerAccountId, opt => opt.MapFrom(src => src.FollowerAccountId))
            .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FollowerAccount.FirstName))
            .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.FollowerAccount.LastName))
            .ForMember(dest => dest.AvatarUrl, opt => opt.MapFrom(src => src.FollowerAccount.AvatarUrl))
            .ReverseMap();
    }   
}
