using MediatR;

namespace Gigbuds_BE.Application.Features.Embedding.JobPostEmbedding
{
    public class JobPostEmbeddingRequest : IRequest
    {
        public required int JobPostId { get; init; }
        public required string JobDescription { get; init; }
        public required string JobRequirement { get; init; }
        public required string ExperienceRequirement { get; init; }
        public bool IsMale { get; init; }
    }
}
