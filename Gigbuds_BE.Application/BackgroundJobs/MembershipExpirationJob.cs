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
        var accountId = context.JobDetail.JobDataMap.GetInt("accountId");
        var membershipId = context.JobDetail.JobDataMap.GetInt("membershipId");
        try{
            
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
