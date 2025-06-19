using Gigbuds_BE.Application.Interfaces.Repositories;
using Gigbuds_BE.Application.Specifications.JobPosts;
using Gigbuds_BE.Domain.Entities.Jobs;
using Gigbuds_BE.Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Gigbuds_BE.Application.Features.JobPosts.Commands.UpdateJobPostStatus
{
    public class UpdateJobPostStatusCommandHandler : IRequestHandler<UpdateJobPostStatusCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<UpdateJobPostStatusCommandHandler> _logger;

        public UpdateJobPostStatusCommandHandler(IUnitOfWork unitOfWork, ILogger<UpdateJobPostStatusCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<int> Handle(UpdateJobPostStatusCommand request, CancellationToken cancellationToken)
        {
            var jobPost = await _unitOfWork.Repository<JobPost>()
                .GetBySpecificationAsync(new JobPostByIdSpecification(request.JobPostId), asNoTracking: false)
                ?? throw new NotFoundException("Job post not found");

            jobPost.JobPostStatus = Enum.Parse<JobPostStatus>(request.Status);

            _unitOfWork.Repository<JobPost>().Update(jobPost);

            try
            {
                await _unitOfWork.CompleteAsync();
                _logger.LogInformation("Job post status with id {JobPostId} updated successfully", jobPost.Id);
                return jobPost.Id;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to update job post status with id {JobPostId}", jobPost.Id);
                throw new UpdateFailedException(nameof(JobPost));
            }
        }
    }
}