using System;
using Gigbuds_BE.Application.Interfaces.Services;
using Microsoft.Extensions.Logging;
using Quartz;

namespace Gigbuds_BE.Application.BackgroundJobs;

[DisallowConcurrentExecution]
public class MembershipExpirationJob : IJob
{
    private readonly IMembershipsService _membershipsService;
    private readonly ILogger<MembershipExpirationJob> _logger;
    public MembershipExpirationJob(IMembershipsService membershipsService, ILogger<MembershipExpirationJob> logger)
    {
        _membershipsService = membershipsService;
        _logger = logger;
    }
    public async Task Execute(IJobExecutionContext context)
    {
        var accountId = int.Parse(context.JobDetail.JobDataMap.GetString("accountId"));
        var membershipId = int.Parse(context.JobDetail.JobDataMap.GetString("membershipId"));
        try{
            _logger.LogInformation("Revoking membership for User {UserId}, Membership {MembershipId}", accountId, membershipId);
            await _membershipsService.RevokeMembershipAsync(accountId, membershipId);

        }catch(Exception ex){

            _logger.LogError(ex, "Failed to revoke membership for User {UserId}, Membership {MembershipId}", accountId, membershipId);

            var jobException = new JobExecutionException(ex){
                RefireImmediately = false
            };
            throw jobException;
        }
    }
}
