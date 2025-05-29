using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gigbuds_BE.Application.Interfaces.Repositories;
using Gigbuds_BE.Application.Specifications.JobPosts;
using MediatR;
using Gigbuds_BE.Domain.Exceptions;
using Gigbuds_BE.Domain.Entities.Jobs;
using Microsoft.Extensions.Logging;

namespace Gigbuds_BE.Application.Features.JobPosts.Commands.RemoveJobPost
{
    public class RemoveJobPostCommandHandler : IRequestHandler<RemoveJobPostCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<RemoveJobPostCommandHandler> _logger;


        public RemoveJobPostCommandHandler(IUnitOfWork unitOfWork, ILogger<RemoveJobPostCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task Handle(RemoveJobPostCommand request, CancellationToken cancellationToken)
        {
            var jobPost = await _unitOfWork.Repository<JobPost>()
                .GetBySpecificationAsync(new JobPostByIdSpecification(request.JobPostId));
            if (jobPost == null)
            {
                throw new NotFoundException("Job post not found");
            }

            jobPost.IsEnabled = false;

            try
            {
                await _unitOfWork.CompleteAsync();
                _logger.LogInformation("Job post with id {JobPostId} removed successfully", jobPost.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to remove job post with id {JobPostId}", jobPost.Id);
                throw new RemoveFailedException(nameof(JobPost));
            }
        }
    }
}