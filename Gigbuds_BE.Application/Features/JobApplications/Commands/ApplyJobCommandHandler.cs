using Gigbuds_BE.Application.DTOs.Files;
using Gigbuds_BE.Application.DTOs.JobApplications;
using Gigbuds_BE.Application.Interfaces.Repositories;
using Gigbuds_BE.Application.Interfaces.Services;
using Gigbuds_BE.Application.Specifications.JobApplications;
using Gigbuds_BE.Application.Specifications.JobPosts;
using Gigbuds_BE.Domain.Entities.Jobs;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Gigbuds_BE.Application.Features.JobApplications.Commands
{
    public class ApplyJobCommandHandler : IRequestHandler<ApplyJobCommand, JobApplicationResponseDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFileStorageService _fileStorageService;
        private readonly ILogger<ApplyJobCommandHandler> _logger;

        public ApplyJobCommandHandler(IUnitOfWork unitOfWork, IFileStorageService fileStorageService, ILogger<ApplyJobCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _fileStorageService = fileStorageService;
            _logger = logger;
        }

        public async Task<JobApplicationResponseDto> Handle(ApplyJobCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // Check if user already applied for this job
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
                var newJobApplication = new JobApplication
                {
                    JobPostId = request.JobApplication.JobPostId,
                    AccountId = request.JobApplication.AccountId,
                    CvUrl = cvUrl,
                    ApplicationStatus = JobApplicationStatus.Pending
                };

                var savedApplication = _unitOfWork.Repository<JobApplication>().Insert(newJobApplication);
                await _unitOfWork.CompleteAsync();

                _logger.LogInformation("Job application created successfully. ApplicationId: {ApplicationId}", savedApplication.Id);

                // Return response
                return new JobApplicationResponseDto
                {
                    Id = savedApplication.Id,
                    JobPostId = savedApplication.JobPostId,
                    AccountId = savedApplication.AccountId,
                    CvUrl = savedApplication.CvUrl,
                    ApplicationStatus = savedApplication.ApplicationStatus.ToString(),
                    AppliedAt = savedApplication.CreatedAt
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing job application. JobPostId: {JobPostId}, AccountId: {AccountId}", 
                    request.JobApplication.JobPostId, request.JobApplication.AccountId);
                throw;
            }
        }
    }
}
