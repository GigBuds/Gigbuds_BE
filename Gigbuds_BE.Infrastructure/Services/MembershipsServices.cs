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

namespace Gigbuds_BE.Infrastructure.Services;

public class MembershipsServices : IMembershipsService
    {
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IApplicationUserService<ApplicationUser> _applicationUserService;
    //private readonly ISchedulerFactory _schedulerFactory;
    private readonly ILogger<MembershipsServices> _logger;
    public MembershipsServices(UserManager<ApplicationUser> userManager, IUnitOfWork unitOfWork, IApplicationUserService<ApplicationUser> applicationUserService, ILogger<MembershipsServices> logger)
    {
        _userManager = userManager;
        _unitOfWork = unitOfWork;
        _applicationUserService = applicationUserService;
        _logger = logger;
    }
    public async Task<bool> CreateMemberShipBenefitsAsync(int accountId, Membership membership) {

        if (membership.MembershipType == MembershipType.JobSeeker
        && membership.Title == ProjectConstant.Basic_Tier_Job_Application_Title) {
            return true;
        }

        if (membership.MembershipType == MembershipType.JobSeeker
        && membership.Title == ProjectConstant.Premium_Tier_Job_Application_Title) {
            return await ActivatePremiumTierJobSeekerMembershipAsync(accountId, membership);
        }

        if(membership.MembershipType == MembershipType.Employer
        && membership.Title == ProjectConstant.Free_Tier_Job_Application_Title) {
            return true;
        }

        if(membership.MembershipType == MembershipType.Employer
        && membership.Title == ProjectConstant.Basic_Tier_Job_Application_Title) {
            return await ActivateBasicTierEmployerMembershipAsync(accountId, membership);
        }

        if(membership.MembershipType == MembershipType.Employer && membership.Title == ProjectConstant.Premium_Tier_Job_Application_Title) {
            return await ActivatePremiumTierEmployerMembershipAsync(accountId, membership);
        }
        return false;
    }

    private async Task<bool> ActivatePremiumTierEmployerMembershipAsync(int accountId, Membership membership)
    {
        var spec = new EmployerByIdSpecification(accountId);
        var employer = await _applicationUserService.GetUserWithSpec(spec);

        if(employer == null) {
            return false;
        }

        employer.EmployerProfile.IsUnlimitedPost = true;

        var jobPostSpec = new GetJobPostByEmployerIdSpecification(accountId);
        var jobPosts = await _unitOfWork.Repository<JobPost>().GetAllWithSpecificationAsync(jobPostSpec, false);
        
        foreach (var jobPost in jobPosts) {
            jobPost.PriorityLevel = 2;
        }
        await _unitOfWork.CompleteAsync();
        return true;
    }

    private async Task<bool> ActivatePremiumTierJobSeekerMembershipAsync(int accountId, Membership membership) {
        var spec = new GetJobApplicationsByAccountIdSpecification(accountId);
        var jobApplications = await _unitOfWork.Repository<JobApplication>().GetAllWithSpecificationAsync(spec, false);

        foreach (var jobApplication in jobApplications) {
            jobApplication.PriorityLevel = 1;
        }

        await _unitOfWork.CompleteAsync();
        return true;
    }

    private async Task<bool> ActivateBasicTierEmployerMembershipAsync(int accountId, Membership membership) {
        var spec = new EmployerByIdSpecification(accountId);
        var employer = await _applicationUserService.GetUserWithSpec(spec);

        if(employer == null) {
            return false;
        }

        employer.EmployerProfile.NumOfAvailablePost = 10;
        employer.EmployerProfile.IsUnlimitedPost = false;
        
        var jobPostSpec = new GetJobPostByEmployerIdSpecification(accountId);
        var jobPosts = await _unitOfWork.Repository<JobPost>().GetAllWithSpecificationAsync(jobPostSpec, false);
        
        foreach (var jobPost in jobPosts) {
            jobPost.PriorityLevel = 1;
        }

        await _unitOfWork.CompleteAsync();
        return true;

    }

    public async Task ScheduleMembershipExpirationAsync(int accountId, Membership membership)
    {
        //try{
        //    var scheduler = await _schedulerFactory.GetScheduler();

        //    var jobKey = new JobKey($"MembershipExpiration_{accountId}_{membership.Id}", "MembershipExpiration");
        //    var triggerKey = new TriggerKey($"MembershipExpirationTrigger_{accountId}_{membership.Id}", "MembershipExpiration");

        //    var job = JobBuilder.Create<MembershipExpirationJob>()
        //    .WithIdentity(jobKey)
        //    .WithDescription("Membership expiration job")
        //    .UsingJobData("accountId", accountId)
        //    .UsingJobData("membershipId", membership.Id)
        //    .Build();

        //    var trigger = TriggerBuilder.Create()
        //    .WithIdentity(triggerKey)
        //    .WithDescription("Membership expiration trigger")
        //    .StartAt(DateTime.UtcNow.AddDays(membership.Duration))
        //    .Build();
            
        //    await scheduler.ScheduleJob(job, trigger);
            
        //    _logger.LogInformation("Scheduled membership expiration job for User {UserId}, Membership {MembershipId}", 
        //        accountId, membership.Id);
        //} catch(Exception ex){
        //    throw new Exception("Failed to schedule membership expiration", ex);
        //}
        
    }

    public async Task RevokeMembershipAsync(int accountId, int membershipId)
    {
        var spec = new GetMembershipByIdSpecification(membershipId);
        var membership = await _unitOfWork.Repository<Membership>().GetBySpecificationAsync(spec);

        var deleteAccountMembership = await _unitOfWork.Repository<AccountMembership>().GetBySpecificationAsync(new GetAccountMembershipByAccountIdAndMembershipIdSpecification(accountId, membershipId));

        if (deleteAccountMembership != null)
        {
            _unitOfWork.Repository<AccountMembership>().Delete(deleteAccountMembership);
        }
        await _unitOfWork.CompleteAsync();

        if (membership.MembershipType == MembershipType.JobSeeker)
        {

            var accountMembershipSpec = new GetAccountMembershipForRevokeSpecification(accountId, membershipId, MembershipType.JobSeeker);

            var accountMembership = await _unitOfWork.Repository<AccountMembership>().GetBySpecificationAsync(accountMembershipSpec, false);

            if (accountMembership != null)
            {
                accountMembership.StartDate = DateTime.UtcNow;
                accountMembership.EndDate = DateTime.UtcNow.AddDays(membership.Duration);
            }
            else
            {
                throw new Exception("Free tier membership not found");
            }
            await _unitOfWork.CompleteAsync();
        }

        if (membership.MembershipType == MembershipType.Employer)
        {

            var accountMembershipSpec = new GetAccountMembershipForRevokeSpecification(accountId, membershipId, MembershipType.Employer);

            var accountMembership = await _unitOfWork.Repository<AccountMembership>().GetBySpecificationAsync(accountMembershipSpec, false);

            if (accountMembership != null)
            {
                accountMembership.StartDate = DateTime.UtcNow;
                accountMembership.EndDate = DateTime.UtcNow.AddDays(membership.Duration);
            }
            else
            {
                throw new Exception("Free tier membership not found");
            }
            await _unitOfWork.CompleteAsync();
        }


    }
}