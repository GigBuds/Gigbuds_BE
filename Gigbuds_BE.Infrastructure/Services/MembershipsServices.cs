using System;
using Gigbuds_BE.Application.Commons.Constants;
using Gigbuds_BE.Application.Interfaces.Services;
using Gigbuds_BE.Domain.Entities.Memberships;
using Gigbuds_BE.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Gigbuds_BE.Application.Interfaces.Repositories;
using Gigbuds_BE.Application.Specifications.JobApplications;
using Gigbuds_BE.Domain.Entities.Jobs;
using Gigbuds_BE.Application.Specifications.ApplicationUsers;
using Gigbuds_BE.Application.Specifications.JobPosts;
using Quartz;
using Microsoft.Extensions.Logging;
using Gigbuds_BE.Application.BackgroundJobs;
using Gigbuds_BE.Application.Specifications.Memberships;
using Gigbuds_BE.Application.DTOs.Memberships;
using AutoMapper;
using Gigbuds_BE.Domain.Entities.Transactions;
using Gigbuds_BE.Application.Specifications.Transactions;

namespace Gigbuds_BE.Infrastructure.Services;

public class MembershipsServices : IMembershipsService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IApplicationUserService<ApplicationUser> _applicationUserService;
    private readonly ISchedulerFactory _schedulerFactory;
    private readonly ILogger<MembershipsServices> _logger;
    private readonly IMapper _mapper;
    public MembershipsServices(IUnitOfWork unitOfWork, IApplicationUserService<ApplicationUser> applicationUserService, ISchedulerFactory schedulerFactory, ILogger<MembershipsServices> logger, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _applicationUserService = applicationUserService;
        _logger = logger;
        _mapper = mapper;
        _schedulerFactory = schedulerFactory;
    }
    public async Task<MembershipResponseDto> CreateMemberShipBenefitsAsync(int accountId, Membership membership)
    {
        await DisableCurrentMembershipAsync(accountId, membership.MembershipType);

        var accountMembership = new AccountMembership
        {
            AccountId = accountId,
            MembershipId = membership.Id,
            StartDate = DateTime.UtcNow,
            EndDate = DateTime.UtcNow.AddDays(membership.Duration),
            Status = AccountMembershipStatus.Active
        };
        _unitOfWork.Repository<AccountMembership>().Insert(accountMembership);
        await _unitOfWork.CompleteAsync();

        if(membership.MembershipType == MembershipType.JobSeeker && membership.Title == ProjectConstant.MembershipLevel.Free_Tier_Job_Application_Title) {
            await ActivateJobSeekerFreeTierMembershipAsync(accountId);
        }
        
        if(membership.MembershipType == MembershipType.Employer && membership.Title == ProjectConstant.MembershipLevel.Free_Tier_Job_Application_Title) {
            await ActivateEmployerFreeTierMembershipAsync(accountId);
        }

        if (membership.MembershipType == MembershipType.JobSeeker
        && membership.Title == ProjectConstant.MembershipLevel.Basic_Tier_Job_Application_Title)
        {
            _logger.LogInformation("Basic tier job seeker membership activated");
        }

        if (membership.MembershipType == MembershipType.JobSeeker
        && membership.Title == ProjectConstant.MembershipLevel.Premium_Tier_Job_Application_Title)
        {
            await ActivatePremiumTierJobSeekerMembershipAsync(accountId, membership);
        }

        if (membership.MembershipType == MembershipType.Employer
        && membership.Title == ProjectConstant.MembershipLevel.Free_Tier_Job_Application_Title)
        {
            _logger.LogInformation("Free tier job seeker membership activated");
        }

        if (membership.MembershipType == MembershipType.Employer
        && membership.Title == ProjectConstant.MembershipLevel.Basic_Tier_Job_Application_Title)
        {
            await ActivateBasicTierEmployerMembershipAsync(accountId, membership);
        }

        if (membership.MembershipType == MembershipType.Employer && membership.Title == ProjectConstant.MembershipLevel.Premium_Tier_Job_Application_Title)
        {
            await ActivatePremiumTierEmployerMembershipAsync(accountId, membership);
        }
        return _mapper.Map<MembershipResponseDto>(accountMembership);
    }

    private async Task<bool> ActivatePremiumTierEmployerMembershipAsync(int accountId, Membership membership)
    {
        var spec = new EmployerByIdSpecification(accountId);
        var employer = await _applicationUserService.GetUserWithSpec(spec);

        if (employer == null)
        {
            return false;
        }

        employer.EmployerProfile.IsUnlimitedPost = true;

        var jobPostSpec = new GetJobPostByEmployerIdSpecification(accountId);
        var jobPosts = await _unitOfWork.Repository<JobPost>().GetAllWithSpecificationAsync(jobPostSpec);

        foreach (var jobPost in jobPosts)
        {
            jobPost.PriorityLevel = ProjectConstant.EmployerMembership.GetPriorityLevel(ProjectConstant.MembershipLevel.Premium_Tier_Job_Application_Title);
            _unitOfWork.Repository<JobPost>().Update(jobPost);
        }
        await _unitOfWork.CompleteAsync();
        return true;
    }

    private async Task<bool> DisableCurrentMembershipAsync(int accountId, MembershipType membershipType)
    {
        var spec = new GetAccountMembershipByAccountIdAndMembershipTypeSpecification(accountId, membershipType);
        var accountMembership = await _unitOfWork.Repository<AccountMembership>().GetAllWithSpecificationAsync(spec);
        
        if(accountMembership.Count == 0) {
            return false;
        }

        foreach (var accountMembershipItem in accountMembership)
        {
            accountMembershipItem.Status = AccountMembershipStatus.Inactive;
            _unitOfWork.Repository<AccountMembership>().Update(accountMembershipItem);
        }

        await _unitOfWork.CompleteAsync();
        return true;
    }

    private async Task<bool> ActivatePremiumTierJobSeekerMembershipAsync(int accountId, Membership membership)
    {
        var spec = new GetJobApplicationsByAccountIdSpecification(accountId);
        var jobApplications = await _unitOfWork.Repository<JobApplication>().GetAllWithSpecificationAsync(spec, false);

        foreach (var jobApplication in jobApplications)
        {
            jobApplication.PriorityLevel = ProjectConstant.JobSeekerMembership.Basic_Job_Application_Priority_Level;
        }

        await _unitOfWork.CompleteAsync();
        return true;
    }

    private async Task<bool> ActivateBasicTierEmployerMembershipAsync(int accountId, Membership membership)
    {
        var spec = new EmployerByIdSpecification(accountId);
        var employer = await _applicationUserService.GetUserWithSpec(spec);

        if (employer == null)
        {
            return false;
        }

        employer.EmployerProfile.NumOfAvailablePost = ProjectConstant.EmployerMembership.Basic_Tier_Job_Post;
        employer.EmployerProfile.IsUnlimitedPost = false;

        var jobPostSpec = new GetJobPostByEmployerIdSpecification(accountId);
        var jobPosts = await _unitOfWork.Repository<JobPost>().GetAllWithSpecificationAsync(jobPostSpec, false);

        foreach (var jobPost in jobPosts)
        {
            jobPost.PriorityLevel = ProjectConstant.EmployerMembership.GetPriorityLevel(ProjectConstant.MembershipLevel.Basic_Tier_Job_Application_Title);
        }

        await _unitOfWork.CompleteAsync();
        return true;

    }

    public async Task ScheduleMembershipExpirationAsync(int accountId, Membership membership)
    {
        try
        {
            var scheduler = await _schedulerFactory.GetScheduler();

            var jobKey = new JobKey($"MembershipExpiration_{accountId}_{membership.Id}", "MembershipExpiration");
            var triggerKey = new TriggerKey($"MembershipExpirationTrigger_{accountId}_{membership.Id}", "MembershipExpiration");

            var job = JobBuilder.Create<MembershipExpirationJob>()
            .WithIdentity(jobKey)
            .WithDescription("Membership expiration job")
            .UsingJobData("accountId", accountId.ToString())
            .UsingJobData("membershipId", membership.Id.ToString())
            .Build();

            var trigger = TriggerBuilder.Create()
            .WithIdentity(triggerKey)
            .WithDescription("Membership expiration trigger")
            .StartAt(DateTime.UtcNow.AddDays(membership.Duration))
            .Build();

            //var trigger = TriggerBuilder.Create()
            //.WithIdentity(triggerKey)
            //.WithDescription("Membership expiration trigger")
            //.StartAt(DateTime.UtcNow.AddMinutes(1))
            //.Build();

            await scheduler.ScheduleJob(job, trigger);

            _logger.LogInformation("Scheduled membership expiration job for User {UserId}, Membership {MembershipId}",
                accountId, membership.Id);
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to schedule membership expiration", ex);
        }

    }

    public async Task RevokeMembershipAsync(int accountId, int membershipId)
    {
        var deleteAccountMembership = await _unitOfWork.Repository<AccountMembership>().GetBySpecificationAsync(new GetAccountMembershipByAccountIdAndMembershipIdSpecification(accountId, membershipId));

        if (deleteAccountMembership != null)
        {
            _unitOfWork.Repository<AccountMembership>().Delete(deleteAccountMembership);
        }

        if (deleteAccountMembership.Membership.MembershipType.ToString() == MembershipType.JobSeeker.ToString())
        {
            await ActivateJobSeekerFreeTierMembershipAsync(accountId);
        }

        if (deleteAccountMembership.Membership.MembershipType.ToString() == MembershipType.Employer.ToString())
        {
            _logger.LogInformation("Cron job revoke membership " + accountId + ":" + membershipId);
            await ActivateEmployerFreeTierMembershipAsync(accountId);
        }

        await _unitOfWork.CompleteAsync();
    }

    private async Task ActivateEmployerFreeTierMembershipAsync(int accountId)
    {
        _logger.LogInformation("ActivateEmployerFreeTierMembershipAsync : " + accountId);
        var account = await _applicationUserService.GetUserWithSpec(new EmployerByIdSpecification(accountId));

        var spec = new GetJobPostByEmployerIdSpecification(accountId);
        var jobPosts = await _unitOfWork.Repository<JobPost>().GetAllWithSpecificationAsync(spec);

        foreach (var jobPost in jobPosts)
        {
            jobPost.PriorityLevel = ProjectConstant.Default_Priority_Level;
            _unitOfWork.Repository<JobPost>().Update(jobPost);
        }

        if (account == null)
        {
            throw new Exception("Account not found");
        }

        account.EmployerProfile.NumOfAvailablePost = ProjectConstant.EmployerMembership.Free_Tier_Job_Post;
        account.EmployerProfile.IsUnlimitedPost = false;
    }

    private async Task ActivateJobSeekerFreeTierMembershipAsync(int accountId)
    {
        var account = await _applicationUserService.GetUserWithSpec(new JobSeekerByIdSpecification(accountId));
        var spec = new GetJobApplicationsByAccountIdSpecification(accountId);
        var jobApplications = await _unitOfWork.Repository<JobApplication>().GetAllWithSpecificationAsync(spec);

        foreach (var jobApplication in jobApplications)
        {
            jobApplication.PriorityLevel = ProjectConstant.Default_Priority_Level;
        }

        if (account == null)
        {
            throw new Exception("Account not found");
        }

        account.AvailableJobApplication = ProjectConstant.JobSeekerMembership.Free_Tier_Job_Application;
    }

    public async Task<bool> ProcessMembershipPaymentSuccessAsync(long orderCode)
    {
        try
        {
            // Find the transaction
            var transactions = await _unitOfWork.Repository<TransactionRecord>()
                .GetAllWithSpecificationAsync(new TransactionByReferenceCodeSpecification(orderCode));

            var transaction = transactions.FirstOrDefault();
            if (transaction == null || !transaction.MembershipId.HasValue)
            {
                _logger.LogWarning("Transaction or membership not found for order code {OrderCode}", orderCode);
                return false;
            }

            // Get membership details
            var spec = new GetMembershipByIdSpecification(transaction.MembershipId.Value);
            var membership = await _unitOfWork.Repository<Membership>().GetBySpecificationAsync(spec);

            if (membership == null)
            {
                _logger.LogWarning("Membership not found for ID {MembershipId}", transaction.MembershipId.Value);
                return false;
            }

            // Activate membership
            await CreateMemberShipBenefitsAsync(transaction.AccountId, membership);
            await ScheduleMembershipExpirationAsync(transaction.AccountId, membership);

            _logger.LogInformation("Successfully activated membership for user {UserId}, membership {MembershipId}", 
                transaction.AccountId, membership.Id);

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing membership payment success for order code {OrderCode}", orderCode);
            return false;
        }
    }
}