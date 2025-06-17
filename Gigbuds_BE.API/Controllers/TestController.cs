using AutoMapper;
using Gigbuds_BE.Application.Features.Embedding.JobSeekerEmbedding;
using Gigbuds_BE.Application.Interfaces;
using Gigbuds_BE.Application.Interfaces.Services;
using Gigbuds_BE.Application.Specifications;
using Gigbuds_BE.Domain.Entities.Constants;
using Gigbuds_BE.Domain.Entities.Identity;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Gigbuds_BE.Infrastructure.Services.SignalR;
using Gigbuds_BE.Application.Interfaces.Services.NotificationServices;
using Microsoft.AspNetCore.SignalR;
using Gigbuds_BE.Application.DTOs.Notifications;

namespace Gigbuds_BE.API.Controllers
{
    public class TestController : _BaseApiController
    {
        private readonly IMediator _mediator;
        private readonly IApplicationUserService<ApplicationUser> _applicationUserService;
        private readonly IMapper _mapper;
        private readonly IHubContext<NotificationHub, INotificationForUser> _hubContext;

        public TestController(
            IMediator mediator,
            IApplicationUserService<ApplicationUser> applicationUserService,
            IMapper mapper,
            IHubContext<NotificationHub, INotificationForUser> hubContext)
        {
            _mediator = mediator;
            _applicationUserService = applicationUserService;
            _mapper = mapper;
            _hubContext = hubContext;
        }

        [HttpPost("process-jobseeker-embeddings")]
        public async Task<IActionResult> ProcessJobSeekerEmbeddings()
        {
            try
            {
                // Get all users with JobSeeker role and include necessary navigation properties
                var jobSeekers = await _applicationUserService.GetUsersByRoleAsync(UserRoles.JobSeeker);

                // Get full user details for each job seeker
                foreach (var jobSeeker in jobSeekers)
                {
                    var fullJobSeeker = await _applicationUserService.GetByIdAsync(
                        jobSeeker.Id,
                        new List<string>
                        {
                            "SkillTags",
                            "EducationalLevels",
                            "AccountExperienceTags"
                        });

                    if (fullJobSeeker == null) continue;

                    // Map the job seeker to embedding request using AutoMapper
                    var embeddingRequest = _mapper.Map<JobSeekerEmbeddingRequest>(fullJobSeeker);

                    // Process the embedding request
                    await _mediator.Send(embeddingRequest);
                }

                return Ok(new { message = "Job seeker embeddings processed successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPost("notify-jobseeker-newjob/{id}")]
        public async Task<IActionResult> NotifyJobSeekerOfNewJobPost([FromBody] NewJobPostNotificationDto dto, string id)
        {
            await _hubContext.Clients.User(id).NotifyNewJobPostMatching(dto.Notification);
            return Ok(new { message = "Notification sent to all job seekers." });
        }

        [HttpPost("notify-jobseekers-newjob")]
        public async Task<IActionResult> NotifyJobSeekersOfNewJobPost([FromBody] NewJobPostNotificationDto dto)
        {
            await _hubContext.Clients.All.NotifyNewJobPostMatching(dto.Notification);
            return Ok(new { message = "Notification sent to all job seekers." });
        }

        [HttpPost("notify-employer-newjob/{id}")]
        public async Task<IActionResult> NotifyEmployer([FromBody] NotificationDto notificationDto, [FromRoute] int id)
        {
            await _hubContext.Clients.User(id.ToString()).NotifyNewFollower(notificationDto);
            return Ok(new { message = "Notification sent to all job seekers." });
        }

        public class NewJobPostNotificationDto
        {
            public NotificationDto Notification { get; set; }
        }
    }
}
