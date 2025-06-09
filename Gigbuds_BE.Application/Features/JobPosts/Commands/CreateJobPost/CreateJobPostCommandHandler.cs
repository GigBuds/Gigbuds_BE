using Gigbuds_BE.Application.Interfaces.Repositories;
using Gigbuds_BE.Domain.Entities.Jobs;
using Gigbuds_BE.Domain.Exceptions;
using Microsoft.Extensions.Logging;
using MediatR;
using Gigbuds_BE.Application.Interfaces.Services;
using Gigbuds_BE.Domain.Entities.Identity;
using static Gigbuds_BE.Application.Commons.Constants.ProjectConstant;
using Gigbuds_BE.Application.Features.Notifications;

namespace Gigbuds_BE.Application.Features.JobPosts.Commands.CreateJobPost
{
    public class CreateJobPostCommandHandler : IRequestHandler<CreateJobPostCommand, int>
    {
        private readonly ILogger<CreateJobPostCommandHandler> _logger;
        private readonly IMediator _mediator;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IApplicationUserService<ApplicationUser> _applicationUserService;

        public CreateJobPostCommandHandler(
            ILogger<CreateJobPostCommandHandler> logger,
            IMediator mediator,
            IUnitOfWork unitOfWork,
            IApplicationUserService<ApplicationUser> applicationUserService)
        {
            _logger = logger;
            _mediator = mediator;
            _unitOfWork = unitOfWork;
            _applicationUserService = applicationUserService;
        }

        public async Task<int> Handle(CreateJobPostCommand command, CancellationToken cancellationToken)
        {
            var jobSeekerMembership = await _applicationUserService.GetJobSeekerMembershipLevelAsync(command.AccountId);

            JobPost newJobPost = new()
            {
                AccountId = command.AccountId,
                JobTitle = command.JobTitle,
                AgeRequirement = command.AgeRequirement,
                JobDescription = command.JobDescription,
                JobRequirement = command.JobRequirement,
                ExperienceRequirement = command.ExperienceRequirement,
                Salary = command.Salary,
                SalaryUnit = Enum.Parse<SalaryUnit>(command.SalaryUnit),
                JobLocation = command.JobLocation,
                ExpireTime = command.ExpireTime,
                Benefit = command.Benefit,
                VacancyCount = command.VacancyCount,
                DistrictCode = command.DistrictCode,
                ProvinceCode = command.ProvinceCode,
                JobPositionId = command.JobPositionId,
                IsOutstandingPost = command.IsOutstandingPost,
                PriorityLevel = EmployerMembership.GetPriorityLevel(jobSeekerMembership),
                StartDate = command.StartDate,
                EndDate = command.EndDate,
            };

            _logger.LogInformation("New job post: {JobPost}", newJobPost);

            newJobPost = _unitOfWork.Repository<JobPost>().Insert(newJobPost);
            try
            {
                await _unitOfWork.CompleteAsync();
                _logger.LogInformation("New job post created with id: {Id}", newJobPost.Id);

                command.ScheduleCommand.JobPostId = newJobPost.Id;
                await _mediator.Publish(command.ScheduleCommand, cancellationToken);

                await _mediator.Publish(new NotifyJobSeekersRequest
                {
                    JobPostId = newJobPost.Id,
                    JobPostLocation = newJobPost.JobLocation,
                    MinAgeRequirement = newJobPost.AgeRequirement ?? 0,
                    JobTitle = newJobPost.JobTitle,
                    JobDescription = newJobPost.JobDescription,
                    JobRequirement = newJobPost.JobRequirement,
                    ExperienceRequirement = newJobPost.ExperienceRequirement,
                    IsMaleRequired = newJobPost.IsMale,

                    EmployerId = newJobPost.AccountId,
                    JobDeadline = DateOnly.FromDateTime(newJobPost.ExpireTime),
                    DistrictCode = newJobPost.DistrictCode,
                    ProvinceCode = newJobPost.ProvinceCode
                }, cancellationToken);
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