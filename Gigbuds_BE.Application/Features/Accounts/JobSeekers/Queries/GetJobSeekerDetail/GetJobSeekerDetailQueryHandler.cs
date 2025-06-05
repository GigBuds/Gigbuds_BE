using AutoMapper;
using Gigbuds_BE.Application.DTOs;
using Gigbuds_BE.Application.DTOs.ApplicationUsers;
using Gigbuds_BE.Application.DTOs.Feedbacks;
using Gigbuds_BE.Application.Interfaces.Services;
using Gigbuds_BE.Application.Specifications.ApplicationUsers;
using Gigbuds_BE.Domain.Entities.Feedbacks;
using Gigbuds_BE.Domain.Entities.Identity;
using Gigbuds_BE.Domain.Exceptions;
using MediatR;

namespace Gigbuds_BE.Application.Features.Accounts.JobSeekers.Queries.GetJobSeekerDetail
{
    public class GetJobSeekerDetailQueryHandler : IRequestHandler<GetJobSeekerDetailQuery, JobSeekerDetailDto>
    {
        private readonly IApplicationUserService<ApplicationUser> _applicationUserService;
        private readonly IMapper _mapper;

        public GetJobSeekerDetailQueryHandler(
            IApplicationUserService<ApplicationUser> applicationUserService,
            IMapper mapper)
        {
            _applicationUserService = applicationUserService;
            _mapper = mapper;
        }

        public async Task<JobSeekerDetailDto> Handle(GetJobSeekerDetailQuery request, CancellationToken cancellationToken)
        {
            var specification = new JobSeekerDetailSpecification(request.JobSeekerId);
            var jobSeeker = await _applicationUserService.GetUserWithSpec(specification);

            if (jobSeeker == null)
            {
                throw new NotFoundException($"Job seeker with ID {request.JobSeekerId} not found");
            }

            var result = _mapper.Map<JobSeekerDetailDto>(jobSeeker);

            // Calculate feedback statistics
            var jobSeekerFeedbacks = jobSeeker.Feedbacks
                .Where(f => f.FeedbackType == FeedbackType.EmployerToJobSeeker)
                .ToList();

            result.TotalFeedbacks = jobSeekerFeedbacks.Count;
            result.AverageRating = jobSeekerFeedbacks.Any() 
                ? Math.Round(jobSeekerFeedbacks.Average(f => f.Rating), 1) 
                : 0;

            // Map feedbacks with employer information
            result.Feedbacks = jobSeekerFeedbacks.Select(f => new FeedbackDto
            {
                Id = f.Id,
                EmployerId = f.EmployerId,
                EmployerName = $"{f.Employer.FirstName} {f.Employer.LastName}",
                CompanyName = f.Employer.EmployerProfile?.CompanyName ?? "Unknown Company",
                CompanyLogo = f.Employer.EmployerProfile?.CompanyLogo ?? string.Empty,
                FeedbackType = f.FeedbackType,
                Rating = f.Rating,
                Comment = f.Comment ?? string.Empty,
                CreatedAt = f.CreatedAt,
                JobTitle = f.JobHistory?.JobPost?.JobTitle ?? "Unknown Job"
            }).OrderByDescending(f => f.CreatedAt).ToList();

            // Calculate follower count
            result.FollowerCount = jobSeeker.Followers?.Count ?? 0;

            // Map schedule information
            if (jobSeeker.JobSeekerSchedule?.JobShifts != null)
            {
                result.JobSeekerShifts = jobSeeker.JobSeekerSchedule.JobShifts
                    .Select(shift => _mapper.Map<JobSeekerShiftsDtoMapping>(shift))
                    .OrderBy(s => s.DayOfWeek)
                    .ThenBy(s => s.StartTime)
                    .ToList();
            }

            return result;
        }
    }
} 