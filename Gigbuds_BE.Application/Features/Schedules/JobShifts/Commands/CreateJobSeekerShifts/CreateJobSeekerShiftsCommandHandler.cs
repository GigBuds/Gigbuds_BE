using Gigbuds_BE.Application.Interfaces.Repositories;
using Gigbuds_BE.Domain.Entities.Jobs;
using Microsoft.Extensions.Logging;
using Gigbuds_BE.Domain.Exceptions;
using MediatR;
using Gigbuds_BE.Domain.Entities.Schedule;

namespace Gigbuds_BE.Application.Features.Schedules.JobShifts.Commands.CreateJobShift
{
    public class CreateJobShiftsCommandHandler : INotificationHandler<CreateJobSeekerShiftsCommand>
    {
        private readonly ILogger<CreateJobShiftsCommandHandler> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public CreateJobShiftsCommandHandler(
            ILogger<CreateJobShiftsCommandHandler> logger,
            IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(CreateJobSeekerShiftsCommand command, CancellationToken cancellationToken)
        {
            foreach (var js in command.JobShifts)
            {
                JobSeekerShift newJobShift = new()
                {
                    JobSeekerScheduleId = command.JobSeekerId,
                    StartTime = js.StartTime,
                    EndTime = js.EndTime,
                    DayOfWeek = js.DayOfWeek,
                };
                _logger.LogInformation("New job shift: {JobShift}", newJobShift);
                _unitOfWork.Repository<JobSeekerShift>().Insert(newJobShift);
            }

            try
            {
                int rowsAdded = await _unitOfWork.CompleteAsync();
                _logger.LogInformation("Added {RowsAdded} rows to the database, from {RowsProvided} rows provided", rowsAdded, command.JobShifts.Count);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating a new job post.");
                throw new CreateFailedException(nameof(JobShift));
            }
        }
    }
}
