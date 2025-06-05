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
            CreateMap<AccountMembership, MembershipResponseDto>();
        }
    }
}
