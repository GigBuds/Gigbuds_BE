using Gigbuds_BE.Application.Interfaces.Repositories;
using DomainJobShift = Gigbuds_BE.Domain.Entities.Jobs;
using Microsoft.Extensions.Logging;
using Wolverine;
using Gigbuds_BE.Application.Features.Schedules.JobShifts.Commands.CreateJobShift;
using Gigbuds_BE.Domain.Entities.Jobs;
using Gigbuds_BE.Domain.Exceptions;

namespace Gigbuds_BE.Application.Features.Schedules.Commands.CreateJobPostSchedule
{
    public class CreateJobPostScheduleCommandHandler
    {
        public async Task<JobPostSchedule> Handle(
            CreateJobPostScheduleCommand command,
            ILogger<CreateJobPostScheduleCommandHandler> logger,
            IUnitOfWork unitOfWork,
            IMessageBus messageBus)
        {

            JobPostSchedule newJobPostSchedule = new()
            {
                Id = command.JobPostId,
                ShiftCount = command.ShiftCount,
                MinimumShift = command.MinimumShift,
            };

            unitOfWork.Repository<JobPostSchedule>().Insert(newJobPostSchedule);

            try
            {
                int rowsAdded = await unitOfWork.CompleteAsync();
                logger.LogInformation("Added {RowsAdded} rows to the database, from {RowsProvided} rows provided", rowsAdded, command.JobShifts.Count);

                IReadOnlyList<DomainJobShift.JobShift> jobShifts =
                await messageBus.InvokeAsync<IReadOnlyList<DomainJobShift.JobShift>>(
                    new CreateJobShiftsCommand
                    {
                        JobPostId = command.JobPostId,
                        JobShifts = command.JobShifts
                    });
                return newJobPostSchedule;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while creating a new job post schedule.");
                throw new CreateFailedException(nameof(JobPostSchedule));
            }
        }
    }
}
