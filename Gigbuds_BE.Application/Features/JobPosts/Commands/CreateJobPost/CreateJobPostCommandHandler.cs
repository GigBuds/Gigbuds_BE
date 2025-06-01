using Gigbuds_BE.Application.Features.Schedules.Commands.CreateJobPostSchedule;
using Gigbuds_BE.Application.Interfaces.Repositories;
using Gigbuds_BE.Domain.Entities.Jobs;
using Gigbuds_BE.Domain.Exceptions;
using Microsoft.Extensions.Logging;
using MediatR;

namespace Gigbuds_BE.Application.Features.JobPosts.Commands.CreateJobPost
{
    public class CreateJobPostCommandHandler : IRequestHandler<CreateJobPostCommand, int>
    {
        private readonly ILogger<CreateJobPostCommandHandler> _logger;
        private readonly IMediator _mediator;
        private readonly IUnitOfWork _unitOfWork;

        public CreateJobPostCommandHandler(
            ILogger<CreateJobPostCommandHandler> logger,
            IMediator mediator,
            IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _mediator = mediator;
            _unitOfWork = unitOfWork;
        }

        public async Task<int> Handle(CreateJobPostCommand command, CancellationToken cancellationToken)
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
                DistrictCode = command.DistrictCode,
                ProvinceCode = command.ProvinceCode,

                // TODO: add check memberhsip type -> if not premium, priority level high, else low
                //PriorityLevel = command.Membership == "Premium" ? 1 : 2, // Assuming 1 is high priority and 2 is low priority
            };

            _logger.LogInformation("New job post: {JobPost}", newJobPost);

            newJobPost = _unitOfWork.Repository<JobPost>().Insert(newJobPost);
            try
            {
                await _unitOfWork.CompleteAsync();
                _logger.LogInformation("New job post created with id: {Id}", newJobPost.Id);

                command.ScheduleCommand.JobPostId = newJobPost.Id;
                await _mediator.Publish(command.ScheduleCommand, cancellationToken);
                return newJobPost.Id;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating a new job post.");
                throw new CreateFailedException(nameof(JobPost));
            }
        }
    }
}