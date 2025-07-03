using System;
using Gigbuds_BE.Application.DTOs.Feedbacks;
using Gigbuds_BE.Application.Interfaces.Repositories;
using Gigbuds_BE.Application.Specifications.Feedbacks;
using Gigbuds_BE.Application.Specifications.JobHistories;
using Gigbuds_BE.Domain.Entities.Feedbacks;
using Gigbuds_BE.Domain.Entities.Jobs;
using Gigbuds_BE.Domain.Exceptions;
using MediatR;

namespace Gigbuds_BE.Application.Features.Feedbacks.CreateFeedback;

public class CreateFeedbackCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<CreateFeedbackCommand, CreateFeedbackDto>
{
    public async Task<CreateFeedbackDto> Handle(CreateFeedbackCommand request, CancellationToken cancellationToken)
    {
        var spec = new GetJobHistoryByIdSpecification(request.CreateFeedbackDto.JobHistoryId);
        var jobHistory = await unitOfWork.Repository<JobHistory>().GetBySpecificationAsync(spec, false);

        if (jobHistory == null)
        {
            throw new NotFoundException("Job history not found");
        }

        var isFeedbackAvailable = await CheckFeedbackAvailable(request.CreateFeedbackDto.JobSeekerId, request.CreateFeedbackDto.EmployerId, 
            request.CreateFeedbackDto.FeedbackType, request.CreateFeedbackDto.JobHistoryId);

        if(!isFeedbackAvailable) {
            throw new Exception("Feedback already exists");
        }

        var feedback = new Feedback{
                AccountId = request.CreateFeedbackDto.JobSeekerId,
                EmployerId = request.CreateFeedbackDto.EmployerId,
                JobHistoryId = request.CreateFeedbackDto.JobHistoryId,
                Rating = request.CreateFeedbackDto.Rating,
                Comment = request.CreateFeedbackDto.Comment,
                FeedbackType = request.CreateFeedbackDto.FeedbackType,
                UpdatedAt = DateTime.UtcNow,
                CreatedAt = DateTime.UtcNow
            };
        unitOfWork.Repository<Feedback>().Insert(feedback);

        if(request.CreateFeedbackDto.FeedbackType == FeedbackType.EmployerToJobSeeker) {
            var jobApplication = jobHistory.JobPost.JobApplications.FirstOrDefault(x => x.AccountId == request.CreateFeedbackDto.JobSeekerId);
            if(jobApplication == null) {
                throw new NotFoundException("Job application not found");
            }
            jobApplication.UpdatedAt = DateTime.UtcNow;
            jobApplication.IsFeedback = true;
        } else {
            jobHistory.IsJobSeekerFeedback = true;
        }
        await unitOfWork.CompleteAsync();
        return request.CreateFeedbackDto;
    }

    private async Task<bool> CheckFeedbackAvailable(int jobSeekerId, int employerId, FeedbackType feedbackType, int jobHistoryId)
    {
        var spec = new GetFeedbackForCheckingSpecification(jobSeekerId, employerId, feedbackType, jobHistoryId);
        var feedback = await unitOfWork.Repository<Feedback>().GetBySpecificationAsync(spec);
        if(feedback != null) {
            return false;
        }
        return true;
    }
}
