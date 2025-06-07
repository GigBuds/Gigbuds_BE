using System;
using Gigbuds_BE.Application.DTOs.JobSeekerShifts;
using MediatR;

namespace Gigbuds_BE.Application.Features.JobSeekerShifts.Query;

public class GetAllJobSeekerShiftsQuery : IRequest<List<JobSeekerShiftResponseDtoProjection>>
{
    public int AccountId { get; set; }

    public GetAllJobSeekerShiftsQuery(int accountId)
    {
        AccountId = accountId;
    }
}
