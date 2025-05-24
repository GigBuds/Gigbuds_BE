using Gigbuds_BE.Application.Interfaces.Repositories;
using Gigbuds_BE.Domain.Entities.Jobs;
using Microsoft.Extensions.Logging;
using Gigbuds_BE.Domain.Exceptions;

namespace Gigbuds_BE.Application.Features.Schedules.JobShifts.Commands.CreateJobShift
{
    public class CreateJobShiftsCommandHandler
    {
        public async Task<IReadOnlyList<JobShift>> Handle(
            CreateJobShiftsCommand command,
            ILogger<CreateJobShiftsCommandHandler> logger,
            IUnitOfWork unitOfWork)
        {
            List<JobShift> newJobShifts = new();
            foreach (var js in command.JobShifts)
            {
                JobShift newJobShift = new()
                {
                    JobPostScheduleId = command.JobPostId,
                    StartTime = js.StartTime,
                    EndTime = js.EndTime,
                    DayOfWeek = js.DayOfWeek,
                };
                logger.LogInformation("New job shift: {JobShift}", newJobShift);
                unitOfWork.Repository<JobShift>().Insert(newJobShift);
                newJobShifts.Add(newJobShift);
            }

            try
            {
                int rowsAdded = await unitOfWork.CompleteAsync();
                logger.LogInformation("Added {RowsAdded} rows to the database, from {RowsProvided} rows provided", rowsAdded, command.JobShifts.Count);
                return newJobShifts;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while creating a new job post.");
                throw new CreateFailedException(nameof(JobShift));
            }
        }
    }
}
