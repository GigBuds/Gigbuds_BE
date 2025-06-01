using Gigbuds_BE.Application.DTOs.JobApplications;
using Gigbuds_BE.Application.Features.JobApplications.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gigbuds_BE.API.Controllers
{
    [ApiController]
    public class JobApplicationsController : _BaseApiController
    {
        private readonly IMediator _mediator;
        private readonly ILogger<JobApplicationsController> _logger;

        public JobApplicationsController(IMediator mediator, ILogger<JobApplicationsController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        /// <summary>
        /// Apply for a job with optional CV upload
        /// </summary>
        /// <param name="jobPostId">The ID of the job post to apply for</param>
        /// <param name="accountId">The ID of the account applying</param>
        /// <param name="cvFile">Optional CV file to upload</param>
        /// <returns>Job application result</returns>
        [HttpPost("apply")]
        [RequestSizeLimit(10_000_000)] // 10MB limit
        [Consumes("multipart/form-data")]
        [ProducesResponseType(typeof(JobApplicationResponseDto), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(409)] // Conflict - already applied
        public async Task<IActionResult> ApplyJob(int jobPostId, int accountId, IFormFile? cvFile = null)
        {
            try
            {
                _logger.LogInformation("Job application request received. JobPostId: {JobPostId}, AccountId: {AccountId}, HasCV: {HasCV}",
                    jobPostId, accountId, cvFile != null);

                var jobApplicationDto = new JobApplicationDto
                {
                    JobPostId = jobPostId,
                    AccountId = accountId,
                    CvFile = cvFile
                };

                var command = new ApplyJobCommand(jobApplicationDto);
                var result = await _mediator.Send(command);

                _logger.LogInformation("Job application successful. ApplicationId: {ApplicationId}", result.Id);
                return Ok(result);
            }
            catch (InvalidOperationException ex) when (ex.Message.Contains("already applied"))
            {
                _logger.LogWarning("Duplicate job application attempt. JobPostId: {JobPostId}, AccountId: {AccountId}", 
                    jobPostId, accountId);
                return Conflict(new { error = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning("Job application validation failed: {Message}", ex.Message);
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing job application. JobPostId: {JobPostId}, AccountId: {AccountId}", 
                    jobPostId, accountId);
                return StatusCode(500, new { error = "An error occurred while processing your application" });
            }
        }

        /// <summary>
        /// Get job applications for a specific job post (for employers)
        /// </summary>
        /// <param name="jobPostId">The ID of the job post</param>
        /// <returns>List of job applications</returns>
        [HttpGet("job/{jobPostId}")]
        [ProducesResponseType(typeof(List<JobApplicationResponseDto>), 200)]
        public async Task<IActionResult> GetJobApplicationsByJobPost(int jobPostId)
        {
            // TODO: Implement GetJobApplicationsByJobPostQuery
            return Ok(new List<JobApplicationResponseDto>());
        }

        /// <summary>
        /// Get job applications for a specific user (for job seekers)
        /// </summary>
        /// <param name="accountId">The ID of the account</param>
        /// <returns>List of job applications</returns>
        [HttpGet("user/{accountId}")]
        [ProducesResponseType(typeof(List<JobApplicationResponseDto>), 200)]
        public async Task<IActionResult> GetJobApplicationsByUser(int accountId)
        {
            // TODO: Implement GetJobApplicationsByUserQuery
            return Ok(new List<JobApplicationResponseDto>());
        }
    }
}
