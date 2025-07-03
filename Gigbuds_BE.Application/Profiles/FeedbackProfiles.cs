using System;
using AutoMapper;
using Gigbuds_BE.Application.DTOs.Feedbacks;
using Gigbuds_BE.Domain.Entities.Feedbacks;

namespace Gigbuds_BE.Application.Profiles;

public class FeedbackProfiles : Profile
{
    public FeedbackProfiles()
    {   
        CreateProjection<Feedback, FeedbackDto>()
            .ForMember(dest => dest.AccountName, opt => opt.MapFrom(src => src.Account.FirstName + " " + src.Account.LastName))
            .ForMember(dest => dest.AccountAvatar, opt => opt.MapFrom(src => src.Account.AvatarUrl))
            .ForMember(dest => dest.EmployerName, opt => opt.MapFrom(src => src.Employer.EmployerProfile.CompanyName))
            .ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => src.Employer.EmployerProfile.CompanyName))
            .ForMember(dest => dest.CompanyLogo, opt => opt.MapFrom(src => src.Employer.EmployerProfile.CompanyLogo))
            .ForMember(dest => dest.JobTitle, opt => opt.MapFrom(src => src.JobHistory.JobPost.JobTitle));

    }
}
