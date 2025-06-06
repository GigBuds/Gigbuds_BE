using Gigbuds_BE.Application.Interfaces.Services;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Text;

namespace Gigbuds_BE.Application.Features.Embedding.JobPostEmbedding
{
    internal class JobPostEmbeddingRequestHandler : IRequestHandler<JobPostEmbeddingRequest, List<(string, string)>>
    {
        private readonly ILogger<JobPostEmbeddingRequestHandler> _logger;
        private readonly ITextEmbeddingService _textEmbeddingService;
        private readonly IVectorStorageService _vectorStorageService;
        private readonly IConfiguration _configuration;

        public JobPostEmbeddingRequestHandler(
            ILogger<JobPostEmbeddingRequestHandler> logger,
            ITextEmbeddingService textEmbeddingService,
            IVectorStorageService vectorStorageService,
            IConfiguration configuration)
        {
            _logger = logger;
            _textEmbeddingService = textEmbeddingService;
            _vectorStorageService = vectorStorageService;
            _configuration = configuration;
        }
        public async Task<List<(string, string)>> Handle(JobPostEmbeddingRequest request, CancellationToken cancellationToken)
        {
            StringBuilder promptDescription = new();
            ConvertObjectToStringDescription();
            _logger.LogInformation("Job Seeker Embedding Conversion Prompt: {Prompt}", promptDescription.ToString());

            var embeddings = await _textEmbeddingService.GenerateEmbeddingsAsync(promptDescription.ToString());

            QueryFilter queryFilter = new()
            {
                Must =
                [
                    new() { FieldName = "age", GreaterThan = request.MinAgeRequirement },
                    new() { FieldName = "isMale", MatchBoolValue = request.IsMaleRequired },
                    new() { FieldName = "isEnabled", MatchBoolValue = true }
                ]
            };

            var jobSeekersWithLocation = await _vectorStorageService.SearchBySemanticsAsync(
                _configuration["VectorDb:JobSeekerCollection"]!, embeddings, queryFilter);

            _logger.LogInformation("Found {Count} job seekers matching the job post requirements", jobSeekersWithLocation.Count);
            return jobSeekersWithLocation;

            void ConvertObjectToStringDescription()
            {
                promptDescription.AppendFormat("Thông tin về công việc cần tuyển, tên công việc: {0}. ", request.JobTitle);

                promptDescription.Append("Tại đây, ứng viên cần phải ");
                promptDescription.Append(request.JobDescription.ToLower());
                promptDescription.Append(". ");

                promptDescription.Append("Yêu cầu công việc bao gồm ");
                promptDescription.Append(request.JobRequirement.ToLower());
                promptDescription.Append(". ");

                promptDescription.Append("Về yêu cầu kinh nghiệm, ");
                promptDescription.Append(request.ExperienceRequirement.ToLower());
                promptDescription.Append('.');
            }
        }
    }
}
