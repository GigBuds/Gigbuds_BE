using Gigbuds_BE.Application.Interfaces.Repositories;
using DomainJobShift = Gigbuds_BE.Domain.Entities.Jobs;
using Microsoft.Extensions.Logging;
using Gigbuds_BE.Domain.Exceptions;

namespace Gigbuds_BE.Application.Features.Schedules.JobShifts.Commands.CreateJobShift
{
    internal class CreateJobShiftsCommandHandler
    {
        public async Task<IReadOnlyList<DomainJobShift.JobShift>> Handle(
            CreateJobShiftsCommand command,
            ILogger<CreateJobShiftsCommandHandler> logger,
            IUnitOfWork unitOfWork)
        {
            // [SCI] Hypothesis: Using foreach is more idiomatic and avoids side effects in LINQ. Verification: C# best practices, Microsoft docs.
            var jobShiftRepo = unitOfWork.Repository<DomainJobShift.JobShift>();
            List<DomainJobShift.JobShift> newJobShifts = new();
            foreach (var js in command.JobShifts)
            {
                DomainJobShift.JobShift newJobShift = new()
                {
                    StartTime = js.StartTime,
                    EndTime = js.EndTime,
                    DayOfWeek = js.DayOfWeek,
                };
                logger.LogInformation("New job shift: {JobShift}", newJobShift);
                jobShiftRepo.Insert(newJobShift);
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
                throw new CreateFailedException(nameof(DomainJobShift.JobShift));
            }
        }
    }
}
