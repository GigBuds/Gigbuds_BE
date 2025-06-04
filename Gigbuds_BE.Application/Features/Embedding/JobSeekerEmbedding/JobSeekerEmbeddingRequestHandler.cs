using Gigbuds_BE.Application.Interfaces.Services;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Text;

namespace Gigbuds_BE.Application.Features.Embedding.JobSeekerEmbedding
{
    internal class JobSeekerEmbeddingRequestHandler : IRequestHandler<JobSeekerEmbeddingRequest>
    {
        private readonly ILogger<JobSeekerEmbeddingRequestHandler> _logger;
        private readonly ITextEmbeddingService _textEmbeddingService;
        private readonly IVectorStorageService _vectorStorageService;
        private readonly IConfiguration _configuration;

        public JobSeekerEmbeddingRequestHandler(
            ILogger<JobSeekerEmbeddingRequestHandler> logger,
            ITextEmbeddingService textEmbeddingService,
            IVectorStorageService vectorStorageService,
            IConfiguration configuration)
        {
            _logger = logger;
            _textEmbeddingService = textEmbeddingService;
            _vectorStorageService = vectorStorageService;
            _configuration = configuration;
        }
        public async Task Handle(JobSeekerEmbeddingRequest request, CancellationToken cancellationToken)
        {
            StringBuilder promptDescription = new();
            ConvertObjectToStringDescription();
            _logger.LogInformation("Job Seeker Embedding Conversion Prompt: {Prompt}", promptDescription.ToString());

            var embeddings = await _textEmbeddingService.GenerateEmbeddingsAsync(promptDescription.ToString());

            var vectorWithPayloads = new List<VectorWithPayload>
            {
                new() {
                    Id = request.Id,
                    Vector = embeddings,
                    Payload = new Dictionary<string, object>
                    {
                        { _configuration["Qdrant:DefaultPointId"]!, request.Id },
                        { "isMale", request.IsMale },
                        { "isEnabled", request.IsEnabled },
                        { "location", request.Location }
                    }
                }
            };

            await _vectorStorageService.UpsertPointAsync(_configuration["VectorDb:JobSeekerCollection"]!, vectorWithPayloads);

            void ConvertObjectToStringDescription()
            {
                promptDescription.Append("Age: ");
                promptDescription.AppendFormat("{0} years of age", (DateTime.Now.Year - request.Dob.Year).ToString());

                promptDescription.Append(';');
                promptDescription.Append("Skills: ");
                if (request.SkillTags != null)
                {
                    foreach (var skillTag in request.SkillTags)
                    {
                        promptDescription.AppendFormat("Able to {0}, ", skillTag.SkillName);
                    }
                }

                promptDescription.Append(';');
                promptDescription.Append("Experience: ");
                if (request.AccountExperienceTags != null)
                {
                    foreach (var experienceTag in request.AccountExperienceTags)
                    {
                        promptDescription.AppendFormat("Has {0} years of experience in {1}, ",
                            experienceTag.EndDate.Year - experienceTag.StartDate.Year,
                            experienceTag.JobPosition);
                    }
                }

                promptDescription.Append(';');
                promptDescription.Append("Education: ");
                if (request.EducationalLevels != null)
                {
                    foreach (var educationLevel in request.EducationalLevels)
                    {
                        promptDescription.AppendFormat("Majored in {0} at {1}, from {2} to {3}, ",
                            educationLevel.Major,
                            educationLevel.SchoolName,
                            educationLevel.StartDate.ToString("yyyy-MM-dd"), // yyyy-MM-dd for readability
                            educationLevel.EndDate.ToString("yyyy-MM-dd"));
                    }
                }
            }
        }
    }
}
