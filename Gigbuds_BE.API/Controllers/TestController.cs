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

namespace Gigbuds_BE.API.Controllers
{
    public class TestController : _BaseApiController
    {
        private readonly IMediator _mediator;
        private readonly IApplicationUserService<ApplicationUser> _applicationUserService;
        private readonly IMapper _mapper;

        public TestController(
            IMediator mediator,
            IApplicationUserService<ApplicationUser> applicationUserService,
            IMapper mapper)
        {
            _mediator = mediator;
            _applicationUserService = applicationUserService;
            _mapper = mapper;
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

    }
}
