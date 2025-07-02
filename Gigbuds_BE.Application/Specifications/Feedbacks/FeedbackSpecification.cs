using System;
using Gigbuds_BE.Domain.Entities.Feedbacks;

namespace Gigbuds_BE.Application.Specifications.Feedbacks;

public class GetFeedbackForCheckingSpecification : BaseSpecification<Feedback>
{   
    public GetFeedbackForCheckingSpecification(int jobSeekerId, int employerId, FeedbackType feedbackType) : base(x => x.AccountId == jobSeekerId && x.EmployerId == employerId && x.FeedbackType == feedbackType) {
    }
}

public class GetAllFeedbacksSpecification : BaseSpecification<Feedback>
{
    public GetAllFeedbacksSpecification(FeedbackType feedbackType) : base(x => x.FeedbackType == feedbackType) {
        AddInclude(x => x.Account);
        AddInclude(x => x.Employer.EmployerProfile);
        AddInclude(x => x.JobHistory.JobPost);
    }
}

public class GetFeedbackByAccountIdSpecification : BaseSpecification<Feedback>
{
    public GetFeedbackByAccountIdSpecification(int accountId, FeedbackType feedbackType) : base(x => x.AccountId == accountId && x.FeedbackType == feedbackType) {
        AddInclude(x => x.Account);
        AddInclude(x => x.Employer.EmployerProfile);
    }
}

public class GetFeedbackByIdSpecification : BaseSpecification<Feedback>
{
    public GetFeedbackByIdSpecification(int feedbackId) : base(x => x.Id == feedbackId) {
        AddInclude(x => x.Account);
        AddInclude(x => x.Employer.EmployerProfile);
    }
}
