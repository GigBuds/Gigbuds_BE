using Gigbuds_BE.Application.Commons.Utils;
using Gigbuds_BE.Application.Interfaces.Services;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Text;

namespace Gigbuds_BE.Application.Features.Utils
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
                    Id = request.JobSeeker.Id,
                    Vector = embeddings,
                    Payload = new Dictionary<string, object>
                    {
                        { "jobSeekerId", request.JobSeeker.Id },
                        { "isMale", request.JobSeeker.IsMale },
                        { "isEnabled", request.JobSeeker.IsEnabled }
                    }
                }
            };

            await _vectorStorageService.UpsertPointAsync(_configuration["VectorDb:DefaultCollection"]!, vectorWithPayloads);


            void ConvertObjectToStringDescription()
            {
                promptDescription.AppendFormat("{0} years of age\n", (DateTime.Now.Year - request.JobSeeker.Dob.Year).ToString());

                foreach (var skillTag in request.JobSeeker.SkillTags)
                {
                    promptDescription.AppendFormat("Able to {0}\n", skillTag.SkillName);
                }

                foreach (var experienceTag in request.JobSeeker.accountExperienceTags)
                {
                    promptDescription.AppendFormat("Has {0} years of experience in {1}\n",
                        experienceTag.EndDate.Year - experienceTag.StartDate.Year,
                        experienceTag.JobPosition);
                }

                foreach (var educationLevel in request.JobSeeker.EducationalLevels)
                {
                    promptDescription.AppendFormat("Majored in {0} at {1}, from {2} to {3}\n",
                        educationLevel.Major,
                        educationLevel.SchoolName,
                        educationLevel.StartDate.ToString("yyyy-MM-dd"), // yyyy-MM-dd for readability
                        educationLevel.EndDate.ToString("yyyy-MM-dd"));
                }

                foreach (var shift in request.JobSeeker.JobSeekerShifts)
                {
                    promptDescription.AppendFormat(
                        "On {0}, available from {1} to {2}\n",
                        ConvertIntDayToString.Convert(shift.DayOfWeek),
                        shift.StartTime.ToString("HH:mm"),
                        shift.EndTime.ToString("HH:mm"));
                }

            }
        }
    }
}
