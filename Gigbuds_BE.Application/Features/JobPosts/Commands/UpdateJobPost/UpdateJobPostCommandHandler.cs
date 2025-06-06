using MediatR;
using Gigbuds_BE.Application.Interfaces.Repositories;
using Gigbuds_BE.Domain.Entities.Jobs;
using Gigbuds_BE.Domain.Exceptions;
using Gigbuds_BE.Application.Specifications.JobPosts;
using Microsoft.Build.Framework;
using Microsoft.Extensions.Logging;

namespace Gigbuds_BE.Application.Features.JobPosts.Commands.UpdateJobPost
{
    internal class UpdateJobPostCommandHandler : IRequestHandler<UpdateJobPostCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<UpdateJobPostCommandHandler> _logger;
        public UpdateJobPostCommandHandler(IUnitOfWork unitOfWork, ILogger<UpdateJobPostCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }
        public async Task Handle(UpdateJobPostCommand request, CancellationToken cancellationToken)
        {
            var jobPost = await _unitOfWork.Repository<JobPost>()
            .GetBySpecificationAsync(new JobPostByIdSpecification(request.JobPostId))
            ?? throw new NotFoundException(nameof(JobPost), request.JobPostId);

            jobPost.JobTitle = request.JobTitle;
            jobPost.AgeRequirement = request.AgeRequirement;
            jobPost.JobDescription = request.JobDescription;
            jobPost.JobRequirement = request.JobRequirement;
            jobPost.ExperienceRequirement = request.ExperienceRequirement;
            jobPost.Salary = request.Salary;
            jobPost.SalaryUnit = Enum.Parse<SalaryUnit>(request.SalaryUnit);
            jobPost.JobLocation = request.JobLocation;
            jobPost.ExpireTime = request.ExpireTime.ToUniversalTime();
            jobPost.Benefit = request.Benefit;
            jobPost.VacancyCount = request.VacancyCount;
            jobPost.DistrictCode = request.DistrictCode;
            jobPost.ProvinceCode = request.ProvinceCode;
            jobPost.IsMale = request.IsMale;
            jobPost.IsOutstandingPost = request.IsOutstandingPost;
            jobPost.JobPositionId = request.JobPositionId;

            _unitOfWork.Repository<JobPost>().Update(jobPost);
            _logger.LogInformation("Job post with id {JobPostId} updated successfully", request.JobPostId);
            try
            {
                await _unitOfWork.CompleteAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating job post with id {JobPostId}", request.JobPostId);
                throw new UpdateFailedException(nameof(JobPost));
            }
        }
    }
}
