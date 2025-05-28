using Gigbuds_BE.Application.Interfaces.Repositories;
using DomainJobShift = Gigbuds_BE.Domain.Entities.Jobs;
using Microsoft.Extensions.Logging;
using MediatR;
using Gigbuds_BE.Application.Features.Schedules.JobShifts.Commands.CreateJobShift;
using Gigbuds_BE.Domain.Entities.Jobs;
using Gigbuds_BE.Domain.Exceptions;

namespace Gigbuds_BE.Application.Features.Schedules.Commands.CreateJobPostSchedule
{
    public class CreateJobPostScheduleCommandHandler : INotificationHandler<CreateJobPostScheduleCommand>
    {
        private readonly ILogger<CreateJobPostScheduleCommandHandler> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediator _mediator;

        public CreateJobPostScheduleCommandHandler(
            ILogger<CreateJobPostScheduleCommandHandler> logger,
            IUnitOfWork unitOfWork,
            IMediator mediator)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _mediator = mediator;
        }

        public async Task Handle(CreateJobPostScheduleCommand command, CancellationToken cancellationToken)
        {
            JobPostSchedule newJobPostSchedule = new()
            {
                Id = command.JobPostId,
                ShiftCount = command.ShiftCount,
                MinimumShift = command.MinimumShift,
            };

            _unitOfWork.Repository<JobPostSchedule>().Insert(newJobPostSchedule);

            try
            {
                int rowsAdded = await _unitOfWork.CompleteAsync();
                _logger.LogInformation("Added {RowsAdded} rows to the database, from {RowsProvided} rows provided", rowsAdded, command.JobShifts.Count);

                await _mediator.Publish(
                    new CreateJobShiftsCommand
                    {
                        JobPostId = command.JobPostId,
                        JobShifts = command.JobShifts
                    }, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating a new job post schedule.");
                throw new CreateFailedException(nameof(JobPostSchedule));
            }
        }
    }
}
