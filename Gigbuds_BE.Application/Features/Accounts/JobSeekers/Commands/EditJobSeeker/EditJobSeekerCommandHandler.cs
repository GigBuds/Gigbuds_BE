using AutoMapper;
using Gigbuds_BE.Application.Interfaces.Repositories;
using Gigbuds_BE.Application.Interfaces.Services;
using Gigbuds_BE.Application.Specifications;
using Gigbuds_BE.Application.Specifications.SkillTags;
using Gigbuds_BE.Domain.Entities.Accounts;
using Gigbuds_BE.Domain.Entities.Identity;
using Gigbuds_BE.Domain.Exceptions;
using MediatR;

namespace Gigbuds_BE.Application.Features.Accounts.JobSeekers.Commands.EditJobSeeker
{
    public class EditJobSeekerCommandHandler : IRequestHandler<EditJobSeekerCommand>
    {
        private readonly IApplicationUserService<ApplicationUser> _applicationUserService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public EditJobSeekerCommandHandler(
            IApplicationUserService<ApplicationUser> applicationUserService,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _applicationUserService = applicationUserService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task Handle(EditJobSeekerCommand request, CancellationToken cancellationToken)
        {
            // Get the job seeker with related data
            var jobSeeker = await _applicationUserService.GetByIdAsync(request.JobSeekerId, new List<string>
            {
                "SkillTags",
                "EducationalLevels", 
                "AccountExperienceTags"
            },true);

            if (jobSeeker == null)
            {
                throw new NotFoundException($"Job seeker with ID {request.JobSeekerId} not found");
            }

            // Update personal information
            jobSeeker.FirstName = request.FirstName;
            jobSeeker.LastName = request.LastName;
            jobSeeker.CurrentLocation = request.CurrentLocation;
            jobSeeker.Dob = DateTime.SpecifyKind(request.Dob, DateTimeKind.Utc);
            jobSeeker.IsMale = request.IsMale;
            jobSeeker.UpdatedAt = DateTime.UtcNow;

            // Clear existing collections
            jobSeeker.SkillTags.Clear();
            jobSeeker.EducationalLevels.Clear();
            jobSeeker.AccountExperienceTags.Clear();

            // Add new skill tags
            foreach (var skillTagId in request.SkillTags)
            {
                var skillTagSpec = new SkillTagByIdSpecification(skillTagId);
                var existingSkillTag = await _unitOfWork.Repository<SkillTag>()
                    .GetBySpecificationAsync(skillTagSpec,false);
                
                if (existingSkillTag != null)
                {
                    jobSeeker.SkillTags.Add(existingSkillTag);
                }
            }

            // Add new educational levels
            foreach (var eduDto in request.EducationalLevels)
            {
                var educationalLevel = _mapper.Map<EducationalLevel>(eduDto);
                educationalLevel.AccountId = jobSeeker.Id;
                if (educationalLevel.StartDate.HasValue)
                {
                    educationalLevel.StartDate = DateTime.SpecifyKind(educationalLevel.StartDate.Value, DateTimeKind.Utc);
                }
                if (educationalLevel.EndDate.HasValue)
                {
                    educationalLevel.EndDate = DateTime.SpecifyKind(educationalLevel.EndDate.Value, DateTimeKind.Utc);
                }
                educationalLevel.CreatedAt = DateTime.UtcNow;
                educationalLevel.UpdatedAt = DateTime.UtcNow;
                jobSeeker.EducationalLevels.Add(educationalLevel);
            }

            // Add new experience tags
            foreach (var expDto in request.AccountExperienceTags)
            {
                var experienceTag = _mapper.Map<AccountExperienceTag>(expDto);
                experienceTag.AccountId = jobSeeker.Id;
                jobSeeker.AccountExperienceTags.Add(experienceTag);
            }

            await _unitOfWork.CompleteAsync();
        }
    }
} 