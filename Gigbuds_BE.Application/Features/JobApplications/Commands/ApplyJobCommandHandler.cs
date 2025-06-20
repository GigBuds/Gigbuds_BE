using AutoMapper;
using Gigbuds_BE.Application.Commons.Constants;
using Gigbuds_BE.Application.DTOs.Files;
using Gigbuds_BE.Application.DTOs.JobApplications;
using Gigbuds_BE.Application.DTOs.Notifications;
using Gigbuds_BE.Application.DTOs.SkillTags;
using Gigbuds_BE.Application.Features.Notifications.Commands.CreateNewNotification;
using Gigbuds_BE.Application.Interfaces.Repositories;
using Gigbuds_BE.Application.Interfaces.Services;
using Gigbuds_BE.Application.Interfaces.Services.NotificationServices;
using Gigbuds_BE.Application.Specifications.ApplicationUsers;
using Gigbuds_BE.Application.Specifications.JobApplications;
using Gigbuds_BE.Application.Specifications.JobPosts;
using Gigbuds_BE.Domain.Entities.Identity;
using Gigbuds_BE.Domain.Entities.Jobs;
using Gigbuds_BE.Domain.Entities.Memberships;
using Gigbuds_BE.Domain.Entities.Notifications;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Gigbuds_BE.Application.Features.JobApplications.Commands
{
    public class ApplyJobCommandHandler : IRequestHandler<ApplyJobCommand, JobApplicationResponseDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFileStorageService _fileStorageService;
        private readonly INotificationService _notificationService;
        private readonly ITemplatingService _templatingService;
        private readonly IMediator _mediator;
        private readonly ILogger<ApplyJobCommandHandler> _logger;
        private readonly IMapper _mapper;
        private readonly IApplicationUserService<ApplicationUser> _applicationUserService;


        public ApplyJobCommandHandler(IUnitOfWork unitOfWork, IFileStorageService fileStorageService, INotificationService notificationService, ITemplatingService templatingService, ILogger<ApplyJobCommandHandler> logger, IMapper mapper, IApplicationUserService<ApplicationUser> applicationUserService, IMediator mediator)
        {
            _mediator = mediator;
            _unitOfWork = unitOfWork;
            _fileStorageService = fileStorageService;
            _notificationService = notificationService;
            _templatingService = templatingService;
            _logger = logger;
            _mapper = mapper;
            _applicationUserService = applicationUserService;
        }

        public async Task<JobApplicationResponseDto> Handle(ApplyJobCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var jobSeekerSpec = new JobSeekerByIdSpecification(request.JobApplication.AccountId);
                var jobSeeker = await _applicationUserService.GetUserWithSpec(jobSeekerSpec);
                var checkMembership = await CheckMembership(jobSeeker);


                var getJobApplicationSpec = new GetJobSpecificationById(request.JobApplication.JobPostId, request.JobApplication.AccountId);
                var existingApplication = await _unitOfWork.Repository<JobApplication>().GetBySpecificationAsync(getJobApplicationSpec);

                if (existingApplication != null)
                {
                    throw new InvalidOperationException("You have already applied for this job");
                }

                // Check if the job post exists
                var jobPostSpec = new JobPostByIdSpecification(request.JobApplication.JobPostId);
                var jobPost = await _unitOfWork.Repository<JobPost>().GetBySpecificationAsync(jobPostSpec);
                if (jobPost == null)
                {
                    throw new InvalidOperationException("Job post not found");
                }

                // Check if job post is still open
                if (jobPost.JobPostStatus != JobPostStatus.Open)
                {
                    throw new InvalidOperationException("This job post is no longer accepting applications");
                }

                if(jobPost.VacancyCount == 0)
                {
                    throw new InvalidOperationException("This job post has reach the maximum number of applicants");
                }

                string? cvUrl = null;

                // Upload CV file if provided
                if (request.JobApplication.CvFile != null && request.JobApplication.CvFile.Length > 0)
                {
                    _logger.LogInformation("Uploading CV file for job application. JobPostId: {JobPostId}, AccountId: {AccountId}",
                        request.JobApplication.JobPostId, request.JobApplication.AccountId);

                    var uploadResult = await _fileStorageService.PrepareUploadFileAsync(
                        request.JobApplication.CvFile,
                        FolderType.Files.ToString(),
                        FileType.Document);

                    if (!uploadResult.Success)
                    {
                        throw new InvalidOperationException($"CV upload failed: {uploadResult.ErrorMessage}");
                    }

                    cvUrl = uploadResult.FileUrl;
                    _logger.LogInformation("CV uploaded successfully. URL: {CvUrl}", cvUrl);
                }

                // Create job application
                var jobApplication = _mapper.Map<JobApplication>(request.JobApplication);
                jobApplication.CvUrl = cvUrl;
                jobApplication.ApplicationStatus = JobApplicationStatus.Pending;
                jobApplication.CreatedAt = DateTime.UtcNow;
                jobApplication.UpdatedAt = DateTime.UtcNow;

                var savedApplication = _unitOfWork.Repository<JobApplication>().Insert(jobApplication);
                jobSeeker.AvailableJobApplication--;
                await _applicationUserService.UpdateAsync(jobSeeker);
                await _unitOfWork.CompleteAsync();

                // Notify employer about the new job application
                var template = await _templatingService.ParseTemplate(ContentType.NewJobApplicationReceived, new NewJobApplicationReceivedTemplateModel()
                {
                    JobName = jobPost.JobTitle
                });

                var notificationDto = await _mediator.Send(new CreateNewNotificationCommand
                {
                    UserId = jobPost.AccountId,
                    Message = template,
                    ContentType = ContentType.NewJobApplicationReceived,
                    CreatedAt = DateTime.UtcNow,
                    AdditionalPayload = new Dictionary<string, string> {
                        { "jobPostId", jobPost.Id.ToString() },
                        { "jobSeekerId", jobSeeker.Id.ToString() }
                    }
                }, cancellationToken);

                await _notificationService.NotifyOneEmployer(
                    typeof(INotificationForEmployers).GetMethod(nameof(INotificationForEmployers.NotifyNewJobApplicationReceived))!,
                    jobPost.AccountId.ToString(),
                    notificationDto
                );

                // Return response
                var jobApplicationResponse = _mapper.Map<JobApplicationResponseDto>(savedApplication);
                return jobApplicationResponse;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing job application. JobPostId: {JobPostId}, AccountId: {AccountId}",
                    request.JobApplication.JobPostId, request.JobApplication.AccountId);
                throw;
            }
        }

        private async Task<bool> CheckMembership(ApplicationUser jobSeeker)
        {
            if (jobSeeker.AvailableJobApplication == 0
            &&
            jobSeeker.AccountMemberships.Any(am => am.Membership.Title == ProjectConstant.MembershipLevel.Free_Tier_Job_Application_Title && am.Status == AccountMembershipStatus.Active))
            {
                throw new InvalidOperationException("You have reached the maximum number of job applications");
            }

            return true;
        }
    }
}
