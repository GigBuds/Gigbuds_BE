using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Gigbuds_BE.Application.DTOs.Memberships;
using Gigbuds_BE.Domain.Entities.Memberships;

namespace Gigbuds_BE.Application.Profiles
{
    public class MembershipsProfile : Profile
    {
        public MembershipsProfile()
        {
            CreateMap<AccountMembership, MembershipResponseDto>()
            .ForMember(dest => dest.MembershipId, opt => opt.MapFrom(src => src.MembershipId));

            CreateProjection<Membership, MembershipDetailResponseDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id));

            CreateProjection<AccountMembership, MemberShipByIdDto>()
                .ForMember(dest => dest.MembershipType, opt => opt.MapFrom(src => src.Membership.MembershipType))
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Membership.Title))
                .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.StartDate))
                .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => src.EndDate))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status));
        }
    }
}
