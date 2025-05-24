using Gigbuds_BE.Application.Features.Schedules.Commands.CreateJobPostSchedule;
using Gigbuds_BE.Application.Interfaces.Repositories;
using Gigbuds_BE.Domain.Entities.Jobs;
using Gigbuds_BE.Domain.Exceptions;
using Microsoft.Build.Framework;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Wolverine;
using Wolverine.Attributes;

namespace Gigbuds_BE.Application.Features.JobPosts.Commands.CreateJobPost
{
    [WolverineHandler]
    public class CreateJobPostCommandHandler
    {
        public async Task<int> Handle(
            CreateJobPostCommand command,
            ILogger<CreateJobPostCommandHandler> logger,
            IMessageBus messageBus,
            IUnitOfWork unitOfWork)
        {
            JobPost newJobPost = new()
            {
                AccountId = command.AccountId,
                JobTitle = command.JobTitle,
                JobDescription = command.JobDescription,
                JobRequirement = command.JobRequirement,
                ExperienceRequirement = command.ExperienceRequirement,
                Salary = command.Salary,
                SalaryUnit = Enum.Parse<SalaryUnit>(command.SalaryUnit),
                JobLocation = command.JobLocation,
                ExpireTime = command.ExpireTime,
                Benefit = command.Benefit,
                VacancyCount = command.VacancyCount,
                IsOutstandingPost = command.IsOutstandingPost,
            };

            logger.LogInformation("New job post: {JobPost}", newJobPost);

            newJobPost = unitOfWork.Repository<JobPost>().Insert(newJobPost);
            try
            {
                await unitOfWork.CompleteAsync();
                logger.LogInformation("New job post created with id: {Id}", newJobPost.Id);

                command.ScheduleCommand.JobPostId = newJobPost.Id;
                await messageBus.SendAsync(command.ScheduleCommand);
                return newJobPost.Id;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while creating a new job post.");
                throw new CreateFailedException(nameof(JobPost));
            }
        }
    }
}