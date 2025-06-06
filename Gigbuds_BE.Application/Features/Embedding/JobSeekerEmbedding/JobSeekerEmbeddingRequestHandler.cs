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
                        { _configuration["VectorDb:DefaultPointId"]!, request.Id },
                        { "isMale", request.IsMale },
                        { "age", DateTime.Now.Year - request.Dob.Year },
                        { "isEnabled", request.IsEnabled },
                        { "location", request.Location },
                    }
                }
            };

            await _vectorStorageService.UpsertPointAsync(_configuration["VectorDb:JobSeekerCollection"]!, vectorWithPayloads);

            void ConvertObjectToStringDescription()
            {
                if (request.SkillTags != null && request.SkillTags.Count > 0)
                {
                    promptDescription.Append("Ứng viên có các kỹ năng sau: ");
                    foreach (var skillTag in request.SkillTags)
                    {
                        promptDescription.AppendFormat("{0}, ", skillTag.SkillName);
                    }
                    promptDescription.Append(". ");
                }
                else
                {
                    promptDescription.Append("Ứng viên chưa cung cấp thông tin về kỹ năng. ");
                }

                if (request.AccountExperienceTags != null && request.AccountExperienceTags.Count > 0)
                {
                    promptDescription.Append("Ứng viên có kinh nghiệm làm việc tại ");
                    foreach (var experienceTag in request.AccountExperienceTags)
                    {
                        promptDescription.AppendFormat("{0} năm với vị trí {1}, ",
                            experienceTag.EndDate.Year - experienceTag.StartDate.Year,
                            experienceTag.JobPosition);
                    }
                    promptDescription.Append(". ");
                }
                else
                {
                    promptDescription.Append("Ứng viên chưa có kinh nghiệm làm việc. ");
                }

                if (request.EducationalLevels != null && request.EducationalLevels.Count > 0)
                {
                    promptDescription.Append("Về trình độ học vấn, ứng viên đã ");
                    foreach (var educationLevel in request.EducationalLevels)
                    {
                        promptDescription.AppendFormat("Học chuyên ngành {0} tại {1} từ {2} đến {3}. ",
                            educationLevel.Major,
                            educationLevel.SchoolName,
                            educationLevel.StartDate.ToString("dd/MM/yyyy"),
                            educationLevel.EndDate.ToString("dd/MM/yyyy"));
                    }
                }
                else
                {
                    promptDescription.Append("Ứng viên chưa cung cấp thông tin về học vấn. ");
                }
            }
        }
    }
}
