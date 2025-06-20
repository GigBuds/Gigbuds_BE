using System;
using MediatR;

namespace Gigbuds_BE.Application.Features.Accounts.JobSeekers.Queries.GetJobSeekerLocations;

public class GetLocationQuery : IRequest<string>
{
    public int JobSeekerId { get; set; }
}
