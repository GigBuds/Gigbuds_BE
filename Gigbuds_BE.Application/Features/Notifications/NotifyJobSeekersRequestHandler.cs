using Gigbuds_BE.Application.Features.Embedding.JobPostEmbedding;
using Gigbuds_BE.Application.Interfaces.Services;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Gigbuds_BE.Application.Features.Notifications
{
    internal class NotifyJobSeekersRequestHandler : INotificationHandler<NotifyJobSeekersRequest>
    {
        private readonly ILogger<NotifyJobSeekersRequestHandler> _logger;
        private readonly IMediator _mediator;
        private readonly IGoogleMapsService _googleMapsService;
        private readonly IConfiguration _configuration;


        public NotifyJobSeekersRequestHandler(
            ILogger<NotifyJobSeekersRequestHandler> logger,
            IMediator mediator,
            IGoogleMapsService googleMapsService,
            IConfiguration configuration)
        {
            _logger = logger;
            _mediator = mediator;
            _googleMapsService = googleMapsService;
            _configuration = configuration;
        }

        public async Task Handle(NotifyJobSeekersRequest request, CancellationToken cancellationToken)
        {
            var jobSeekersWithLocation = await _mediator.Send(new JobPostEmbeddingRequest
            {
                JobPostId = request.JobPostId,
                MinAgeRequirement = request.MinAgeRequirement,
                JobTitle = request.JobTitle,
                JobDescription = request.JobDescription,
                JobRequirement = request.JobRequirement,
                ExperienceRequirement = request.ExperienceRequirement,
                IsMaleRequired = request.IsMaleRequired
            }, cancellationToken);

            var distanceResults = await _googleMapsService.CalculateDistancesToMultipleDestinationsAsync(
                request.JobPostLocation, jobSeekersWithLocation.Select(j => j.Item2).ToList());

            var filteredDistanceResults = distanceResults.Where(d => d.DistanceKilometers <= _configuration.GetValue<double>("Notification:DistanceThresholdInKilometers")).ToList();

            var filteredJobSeekers = jobSeekersWithLocation.Join(
                filteredDistanceResults, j => j.Item2, d => d.Destination,
                (j, d) => j);
            //TODO: Send notification to job seekers
            foreach (var jobSeeker in filteredJobSeekers)
            {
                _logger.LogInformation("Sending notification to job seeker {JobSeekerId} with distance {Distance}", jobSeeker.Item1, jobSeeker.Item2);
            }

            _logger.LogInformation("Found {Count} job seekers matching the job post requirements", jobSeekersWithLocation.Count);

        }
    }
}
