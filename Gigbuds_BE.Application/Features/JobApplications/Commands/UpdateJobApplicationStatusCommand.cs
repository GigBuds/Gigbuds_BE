using System;
using Gigbuds_BE.Domain.Entities.Jobs;
using MediatR;

namespace Gigbuds_BE.Application.Features.JobApplications.Commands;

public class UpdateJobApplicationStatusCommand : IRequest<bool>
{
    public int JobApplicationId { get; set; }
    public JobApplicationStatus Status { get; set; }
}
