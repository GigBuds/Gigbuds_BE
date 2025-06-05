
using Gigbuds_BE.Application.DTOs.ApplicationUsers;
using Gigbuds_BE.Application.Features.Embedding.JobSeekerEmbedding;
using Gigbuds_BE.Application.Interfaces.Repositories;
using Gigbuds_BE.Application.Interfaces.Services;
using Gigbuds_BE.Domain.Entities.Identity;
using MediatR;

namespace Gigbuds_BE.Application.Features.Accounts.JobSeekers.Commands.UpdateJobSeeker
{
    public class UpdateJobSeekerCommandHandler : IRequestHandler<UpdateJobSeekerCommand>
    {
        private readonly IMediator _mediator;
        private readonly IApplicationUserService<ApplicationUser> _applicationUserService;
        public UpdateJobSeekerCommandHandler(IMediator mediator, IApplicationUserService<ApplicationUser> applicationUserService)
        {
            _mediator = mediator;
            _applicationUserService = applicationUserService;
        }

        public async Task Handle(UpdateJobSeekerCommand request, CancellationToken cancellationToken)
        {
            var jobSeeker = await _applicationUserService.GetByIdAsync(request.JobSeekerId);

            //TODO: add update logic here 

            await _mediator.Publish(new JobSeekerEmbeddingRequest
            {
                Id = request.JobSeekerId,
                Dob = DateOnly.FromDateTime(jobSeeker!.Dob),
                IsMale = jobSeeker.IsMale,
                IsEnabled = jobSeeker.IsEnabled,
                Location = request.Location,
                SkillTags = request.SkillTags,
                EducationalLevels = request.EducationalLevels,
                AccountExperienceTags = request.AccountExperienceTags
            }, cancellationToken);
        }
    }
}
