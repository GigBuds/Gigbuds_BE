using MediatR;

namespace Gigbuds_BE.Application.Features.Embedding.JobPostEmbedding
{
    public class JobPostEmbeddingRequest : IRequest<List<(string, string)>>
    {
        public required int JobPostId { get; init; }
        public required int MinAgeRequirement { get; init; }
        public required string JobTitle { get; init; }
        public required string JobDescription { get; init; }
        public required string JobRequirement { get; init; }
        public required string ExperienceRequirement { get; init; }
        public required bool IsMaleRequired { get; init; }
    }
}
