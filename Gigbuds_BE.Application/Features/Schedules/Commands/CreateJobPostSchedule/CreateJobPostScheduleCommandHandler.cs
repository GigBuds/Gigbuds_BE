using Gigbuds_BE.Application.Interfaces.Repositories;
using DomainJobShift = Gigbuds_BE.Domain.Entities.Jobs;
using Microsoft.Extensions.Logging;
using Wolverine;
using Gigbuds_BE.Application.Features.Schedules.JobShifts.Commands.CreateJobShift;
using Gigbuds_BE.Domain.Entities.Jobs;

namespace Gigbuds_BE.Application.Features.Schedules.Commands.CreateJobPostSchedule
{
    internal class CreateJobPostScheduleCommandHandler
    {
        public async Task<JobPostSchedule> Handle(
            CreateJobPostScheduleCommand command,
            ILogger<CreateJobPostScheduleCommandHandler> logger,
            IUnitOfWork unitOfWork,
            IMessageBus messageBus)
        {
            return null;
            //IReadOnlyList<DomainJobShift.JobShift> jobShifts =
            //await messageBus.SendAsync<IReadOnlyList<DomainJobShift.JobShift>>(
            //    new CreateJobShiftsCommand
            //    {
            //        JobShifts = command.JobShifts;
            //    });
            //JobPostSchedule newJobPostSchedule = new()
            //{
            //    ShiftCount = command.ShiftCount,
            //    MinimumShift = command.MinimumShift
            //};

            //unitOfWork.Repository<JobPostSchedule>().Insert(newJobPostSchedule);
            //await unitOfWork.CompleteAsync();
            //return newJobPostSchedule;
        }
    }
}
