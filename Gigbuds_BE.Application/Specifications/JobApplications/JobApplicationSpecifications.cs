using System;
using Gigbuds_BE.Application.DTOs.ApplicationUsers;
using Gigbuds_BE.Domain.Entities.Jobs;

namespace Gigbuds_BE.Application.Specifications.JobApplications;

public class GetJobSpecificationById : BaseSpecification<JobApplication>
{
    public GetJobSpecificationById(int id)
        : base(x => x.Id == id)
    {
        AddInclude(x => x.JobPost);
    }

    public GetJobSpecificationById(int JobPostId, int AccountId)
        : base(x => x.JobPostId == JobPostId && x.AccountId == AccountId)
    {
    }
}

public class GetJobApplicationsByJobPostSpecification : BaseSpecification<JobApplication>
{
    public GetJobApplicationsByJobPostSpecification(int jobPostId)
        : base(x => x.JobPostId == jobPostId)
    {
        AddInclude(x => x.JobPost);
        AddInclude(x => x.Account);
        AddInclude(x => x.JobPost.JobPosition);
        AddInclude(x => x.Account.SkillTags);
        AddInclude(x => x.JobPost.JobHistories);
    }
}
public class GetJobApplicationsByJobPostIdSpecification : BaseSpecification<JobApplication>
{
    public GetJobApplicationsByJobPostIdSpecification(int jobPostId)
        : base(x => x.JobPostId == jobPostId)
    {

    }
}

public class GetJobApplicationsByAccountIdSpecification : BaseSpecification<JobApplication>
{
    public GetJobApplicationsByAccountIdSpecification(int accountId)
        : base(x => x.AccountId == accountId)
    {
    }
}

public class GetJobSeekerMyJobSpecification : BaseSpecification<JobApplication>
{
    public GetJobSeekerMyJobSpecification(JobSeekerMyJobRequestDto requestDto) : base(j => j.AccountId == requestDto.JobSeekerId
        && ((requestDto.MyJobType == MyJobType.AppliedJob && j.ApplicationStatus == JobApplicationStatus.Pending)
        || (requestDto.MyJobType == MyJobType.AcceptedJob && j.ApplicationStatus == JobApplicationStatus.Approved)
        || (requestDto.MyJobType == MyJobType.JobHistory && (j.ApplicationStatus == JobApplicationStatus.Removed || (j.JobPost.JobPostStatus == JobPostStatus.Finished && j.ApplicationStatus == JobApplicationStatus.Approved)))
        ))
    {
        AddInclude(j => j.JobPost);
        AddInclude(j => j.JobPost.JobHistories.Where(jh => jh.AccountId == requestDto.JobSeekerId));
        AddInclude(j => j.JobPost.JobPosition);
        AddInclude(j => j.Account.EmployerProfile);
        AddInclude(j => j.JobPost.JobPosition);
    }
}
