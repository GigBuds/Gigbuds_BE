using System;
using Gigbuds_BE.Application.DTOs.ApplicationUsers;

namespace Gigbuds_BE.Application.Specifications.JobPosts;

public class GetJobSeekerMyJobQueryParams : BasePagingParams
{
    public int JobSeekerId { get; set; }
    public MyJobType MyJobType { get; set; }
}
