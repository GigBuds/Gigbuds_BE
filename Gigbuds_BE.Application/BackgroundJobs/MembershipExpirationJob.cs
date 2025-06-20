using System;
using Gigbuds_BE.Application.Features.Notifications.Commands.CreateNewNotification;
using Gigbuds_BE.Application.Interfaces.Repositories;
using Gigbuds_BE.Application.Interfaces.Services;
using Gigbuds_BE.Application.Interfaces.Services.NotificationServices;
using Gigbuds_BE.Application.Specifications.Memberships;
using Gigbuds_BE.Domain.Entities.Memberships;
using Gigbuds_BE.Domain.Entities.Notifications;
using MediatR;
using Microsoft.Extensions.Logging;
using Quartz;

namespace Gigbuds_BE.Application.BackgroundJobs;

[DisallowConcurrentExecution]
public class MembershipExpirationJob : IJob
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMembershipsService _membershipsService;
    private readonly ITemplatingService _templatingService;
    private readonly INotificationService _notificationService;
    private readonly ILogger<MembershipExpirationJob> _logger;
    private readonly IMediator _mediator;
    public MembershipExpirationJob(IUnitOfWork unitOfWork, IMembershipsService membershipsService, ITemplatingService templatingService, INotificationService notificationService, ILogger<MembershipExpirationJob> logger, IMediator mediator)
    {
        _membershipsService = membershipsService;
        _templatingService = templatingService;
        _notificationService = notificationService;
        _logger = logger;
    }
    public async Task Execute(IJobExecutionContext context)
    {
        var accountId = int.Parse(context.JobDetail.JobDataMap.GetString("accountId"));
        var membershipId = int.Parse(context.JobDetail.JobDataMap.GetString("membershipId"));
        try
        {
            _logger.LogInformation("Revoking membership for User {UserId}, Membership {MembershipId}", accountId, membershipId);
            await _membershipsService.RevokeMembershipAsync(accountId, membershipId);

            var membership = await _unitOfWork.Repository<Membership>().GetBySpecificationAsync(new GetMembershipByIdSpecification(membershipId));
            var template = await _templatingService.ParseTemplate(ContentType.MembershipExpired, new MembershipExpiredTemplateModel()
            {
                MembershipName = membership!.Title
            });

            var notificationDto = await _mediator.Send(new CreateNewNotificationCommand
            {
                UserId = accountId,
                Message = template,
                ContentType = ContentType.MembershipExpired,
                CreatedAt = DateTime.UtcNow,
                AdditionalPayload = new Dictionary<string, string> {
                    { "userId", accountId.ToString() }
                }
            });

            //await _notificationService.NotifyOneUser(
            //    typeof(INotificationForUser).GetMethod(nameof(INotificationForUser.NotifyMembershipExpired))!,
            //    accountId.ToString(),
            //    notificationDto
            //);
        }
        catch (Exception ex)
        {

            _logger.LogError(ex, "Failed to revoke membership for User {UserId}, Membership {MembershipId}", accountId, membershipId);

            var jobException = new JobExecutionException(ex)
            {
                RefireImmediately = false
            };
            throw jobException;
        }
    }
}
