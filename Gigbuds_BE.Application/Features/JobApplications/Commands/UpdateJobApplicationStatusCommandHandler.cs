using System.Reflection;
using Gigbuds_BE.Application.DTOs.Notifications;
using Gigbuds_BE.Application.Features.Notifications.Commands.CreateNewNotification;
using Gigbuds_BE.Application.Interfaces.Repositories;
using Gigbuds_BE.Application.Interfaces.Services.NotificationServices;
using Gigbuds_BE.Application.Interfaces.Services;
using Gigbuds_BE.Application.Specifications.JobApplications;
using Gigbuds_BE.Domain.Entities.Jobs;
using Gigbuds_BE.Domain.Entities.Notifications;
using MediatR;
using Gigbuds_BE.Application.Specifications.Notifications;

namespace Gigbuds_BE.Application.Features.JobApplications.Commands;

public class UpdateJobApplicationStatusCommandHandler(
    IUnitOfWork unitOfWork,
    INotificationService notificationService,
    IMediator mediator,
    ITemplatingService templatingService
    ) : IRequestHandler<UpdateJobApplicationStatusCommand, bool>
{
    public async Task<bool> Handle(UpdateJobApplicationStatusCommand request, CancellationToken cancellationToken)
    {
        var spec = new GetJobSpecificationById(request.JobApplicationId);
        var jobApplication = await unitOfWork.Repository<JobApplication>().GetBySpecificationAsync(spec,false);
        if (jobApplication == null)
        {
            return false;
        }
        jobApplication.ApplicationStatus = request.Status;
        if(request.Status == JobApplicationStatus.Approved)
        {
            jobApplication.JobPost.VacancyCount--;
        }
        if(request.Status == JobApplicationStatus.Removed)
        {
            jobApplication.JobPost.VacancyCount++;
        }
        jobApplication.UpdatedAt = DateTime.UtcNow;
        await unitOfWork.CompleteAsync();

        switch (request.Status)
        {
            case JobApplicationStatus.Approved:
                {
                    var template = await templatingService.ParseTemplate(ContentType.JobApplicationAccepted, new JobApplicationAcceptedTemplateModel
                    {
                        JobName = jobApplication.JobPost!.JobTitle,
                    });
                    var notificationDto = await mediator.Send(new CreateNewNotificationCommand
                    {
                        UserId = jobApplication.AccountId,
                        Message = template,
                        ContentType = ContentType.JobApplicationAccepted,
                        JobPostId = jobApplication.JobPostId,
                        CreatedAt = DateTime.UtcNow,
                        AdditionalPayload = new Dictionary<string, string> {
                            { "jobApplicationId", jobApplication.Id.ToString() },
                            { "userId", jobApplication.AccountId.ToString() },
                        }
                    }, cancellationToken);
                    var userDevices = await unitOfWork.Repository<DevicePushNotifications>()
                        .GetAllWithSpecificationAsync(new GetDevicesByUserSpecification(jobApplication.AccountId));
                    await notificationService.NotifyOneJobSeeker(
                        typeof(INotificationForJobSeekers).GetMethod(nameof(INotificationForJobSeekers.NotifyJobApplicationAccepted))!,
                        userDevices.Select(a => a.DeviceToken!)!.ToList(),
                        jobApplication.AccountId.ToString(),
                        notificationDto
                    );
                }
                break;
            case JobApplicationStatus.Rejected:
                {
                    var template = await templatingService.ParseTemplate(ContentType.JobApplicationRejected, new JobApplicationRejectedTemplateModel
                    {
                        JobName = jobApplication.JobPost!.JobTitle,
                    });
                    var notificationDto = await mediator.Send(new CreateNewNotificationCommand
                    {
                        UserId = jobApplication.AccountId,
                        Message = template,
                        ContentType = ContentType.JobApplicationRejected,
                        JobPostId = jobApplication.JobPostId,
                        CreatedAt = DateTime.UtcNow,
                        AdditionalPayload = new Dictionary<string, string> {
                            { "jobApplicationId", jobApplication.Id.ToString() },
                            { "userId", jobApplication.AccountId.ToString() },
                        }
                    }, cancellationToken);
                    var userDevices = await unitOfWork.Repository<DevicePushNotifications>()
                        .GetAllWithSpecificationAsync(new GetDevicesByUserSpecification(jobApplication.AccountId));
                    await notificationService.NotifyOneJobSeeker(
                        typeof(INotificationForJobSeekers).GetMethod(nameof(INotificationForJobSeekers.NotifyJobApplicationRejected))!,
                        userDevices.Select(a => a.DeviceToken!)!.ToList(),
                        jobApplication.AccountId.ToString(),
                        notificationDto
                    );
                }
                break;
            case JobApplicationStatus.Removed:
                {
                    var template = await templatingService.ParseTemplate(ContentType.JobApplicationRemovedFromApproved, new JobApplicationRemovedTemplateModel
                    {
                        JobName = jobApplication.JobPost!.JobTitle,
                    });
                    var notificationDto = await mediator.Send(new CreateNewNotificationCommand
                    {
                        UserId = jobApplication.AccountId,
                        Message = template,
                        ContentType = ContentType.JobApplicationRemovedFromApproved,
                        JobPostId = jobApplication.JobPostId,
                        CreatedAt = DateTime.UtcNow,
                        AdditionalPayload = new Dictionary<string, string> {
                            { "jobApplicationId", jobApplication.Id.ToString() },
                            { "userId", jobApplication.AccountId.ToString() },
                        }
                    }, cancellationToken);

                    var userDevices = await unitOfWork.Repository<DevicePushNotifications>()
                        .GetAllWithSpecificationAsync(new GetDevicesByUserSpecification(jobApplication.AccountId));
                    await notificationService.NotifyOneJobSeeker(
                        typeof(INotificationForJobSeekers).GetMethod(nameof(INotificationForJobSeekers.NotifyJobApplicationAccepted))!,
                        userDevices.Select(a => a.DeviceToken!)!.ToList(),
                        jobApplication.AccountId.ToString(),
                        notificationDto
                    );
                }
                break;
            default:
                break;
        }
        return true;
    }
}
