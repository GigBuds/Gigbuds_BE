using Gigbuds_BE.Application.Features.Notifications.Commands.CreateNewNotification;
using Gigbuds_BE.Application.Interfaces.Repositories;
using Gigbuds_BE.Application.Interfaces.Services;
using Gigbuds_BE.Application.Interfaces.Services.NotificationServices;
using Gigbuds_BE.Application.Specifications.JobApplications;
using Gigbuds_BE.Application.Specifications.JobPosts;
using Gigbuds_BE.Application.Specifications.Notifications;
using Gigbuds_BE.Domain.Entities.Identity;
using Gigbuds_BE.Domain.Entities.Jobs;
using Gigbuds_BE.Domain.Entities.Notifications;
using Gigbuds_BE.Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Gigbuds_BE.Application.Features.JobPosts.Commands.UpdateJobPostStatus
{
    public class UpdateJobPostStatusCommandHandler : IRequestHandler<UpdateJobPostStatusCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<UpdateJobPostStatusCommandHandler> _logger;
        private readonly IApplicationUserService<ApplicationUser> _applicationUserService;
        private readonly ITemplatingService _templatingService;
        private readonly INotificationService _notificationService;
        private readonly IMediator _mediator;

        public UpdateJobPostStatusCommandHandler(IUnitOfWork unitOfWork, ILogger<UpdateJobPostStatusCommandHandler> logger, IApplicationUserService<ApplicationUser> applicationUserService, ITemplatingService templatingService, INotificationService notificationService, IMediator mediator)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _applicationUserService = applicationUserService;
            _templatingService = templatingService;
            _notificationService = notificationService;
            _mediator = mediator;
        }

        public async Task<int> Handle(UpdateJobPostStatusCommand request, CancellationToken cancellationToken)
        {
            var jobPost = await _unitOfWork.Repository<JobPost>()
                .GetBySpecificationAsync(new JobPostByIdSpecification(request.JobPostId), asNoTracking: false)
                ?? throw new NotFoundException("Job post not found");

            jobPost.JobPostStatus = Enum.Parse<JobPostStatus>(request.Status);

            _unitOfWork.Repository<JobPost>().Update(jobPost);

            try
            {
                await _unitOfWork.CompleteAsync();
                _logger.LogInformation("Job post status with id {JobPostId} updated successfully", jobPost.Id);

                // Notify job seekers who applied for this job post
                if (jobPost.JobPostStatus == JobPostStatus.Finished)
                {
                    var applicants = await _unitOfWork.Repository<JobApplication>().GetAllWithSpecificationAsync(new GetJobApplicationsByJobPostIdSpecification(request.JobPostId));

                    var template = await _templatingService.ParseTemplate(ContentType.JobApplicationAccepted, new JobApplicationAcceptedTemplateModel()
                    {
                        JobName = jobPost.JobTitle,
                    });
                    var tasks = applicants.Select(async applicant =>
                    {
                        var notificationDto = await _mediator.Send(new CreateNewNotificationCommand
                        {
                            UserId = applicant.AccountId,
                            Message = template,
                            ContentType = ContentType.JobApplicationAccepted,
                            CreatedAt = DateTime.UtcNow,
                        });

                        var userDevices = await _unitOfWork.Repository<DevicePushNotifications>()
                            .GetAllWithSpecificationAsync(new GetDevicesByUserSpecification(applicant.AccountId));

                        return Task.Run(async () =>
                        {
                            _logger.LogInformation("Notifying job seeker {JobSeekerId} about job post {JobPostId}", applicant.AccountId, jobPost.Id);

                            await _notificationService.NotifyOneUser(
                                typeof(INotificationForJobSeekers).GetMethod(nameof(INotificationForJobSeekers.NotifyJobCompleted))!,
                                userDevices.Select(a => a.DeviceToken!)!.ToList(),
                                applicant.AccountId.ToString(),
                                notificationDto
                            );
                        });
                    });

                    await Task.WhenAll(tasks);
                }
                return jobPost.Id;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to update job post status with id {JobPostId}", jobPost.Id);
                throw new UpdateFailedException(nameof(JobPost));
            }
        }
    }
}