using System;

namespace Gigbuds_BE.Application.DTOs.ApplicationUsers;

public class JobSeekerMyJobRequestDto
{
    public int JobSeekerId { get; set; }
    
    public MyJobType MyJobType { get; set; } = MyJobType.AppliedJob;
    public int PageIndex { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}
public enum MyJobType
{
    AppliedJob,
    AcceptedJob,
    JobHistory
}