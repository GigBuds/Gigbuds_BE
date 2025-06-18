using System;
using Gigbuds_BE.Application.DTOs.ApplicationUsers;
using MediatR;

namespace Gigbuds_BE.Application.Features.Accounts.EmployerProfiles.Queries;

public class GetEmployerProfileQuery : IRequest<EmployerProfileResponseDto>
{
    public int Id { get; set; }
    public GetEmployerProfileQuery(int id) => Id = id;
}
