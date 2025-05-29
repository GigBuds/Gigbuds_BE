using MediatR;

namespace Gigbuds_BE.Application.Features.JobPosts.Commands.RemoveJobPost
{
    public class RemoveJobPostCommand : IRequest
    {
        public int JobPostId { get; set; }
    }
}