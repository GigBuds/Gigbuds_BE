
using System.Text.Json.Serialization;
using Gigbuds_BE.Domain.Entities.Jobs;
using MediatR;

namespace Gigbuds_BE.Application.Features.JobPosts.Commands.UpdateJobPostStatus
{
    public class UpdateJobPostStatusCommand : IRequest<int>
    {
        [JsonIgnore]
        public int JobPostId { get; set; }
        public required string Status { get; set; }
    }
}