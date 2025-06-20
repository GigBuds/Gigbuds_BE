using Gigbuds_BE.Application.DTOs.Notifications;
using Gigbuds_BE.Application.Features.Embedding.JobPostEmbedding;
using Gigbuds_BE.Application.Features.Notifications.Commands.CreateNewNotification;
using Gigbuds_BE.Application.Interfaces.Repositories;
using Gigbuds_BE.Application.Interfaces.Services;
using Gigbuds_BE.Application.Interfaces.Services.NotificationServices;
using Gigbuds_BE.Application.Specifications.EmployerProfiles;
using Gigbuds_BE.Application.Specifications.Notifications;
using Gigbuds_BE.Domain.Entities.Accounts;
using Gigbuds_BE.Domain.Entities.Notifications;
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
        private readonly INotificationService _notificationService;
        private readonly ITemplatingService _templatingService;
        private readonly IUnitOfWork _unitOfWork;

        public NotifyJobSeekersRequestHandler(
            ILogger<NotifyJobSeekersRequestHandler> logger,
            IMediator mediator,
            IGoogleMapsService googleMapsService,
            IConfiguration configuration,
            INotificationService notificationService,
            ITemplatingService templatingService,
            IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _mediator = mediator;
            _googleMapsService = googleMapsService;
            _configuration = configuration;
            _notificationService = notificationService;
            _templatingService = templatingService;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(NotifyJobSeekersRequest request, CancellationToken cancellationToken)
        {
            var jobSeekers = await FilterJobSeekersByDistance();
            if (!jobSeekers.Any())
            {
                _logger.LogInformation("No job seekers found matching the job post requirements");
                return;
            }

            var parsedTemplate = await RetrieveTemplate();

            await SendNotificationToJobSeekers();

            async Task<string> RetrieveTemplate()
            {
                var employerCompany = await _unitOfWork.Repository<EmployerProfile>().GetBySpecificationAsync(
                    new GetEmployerProfleByAccountIdSpecification(request.EmployerId)
                );

                return await _templatingService.ParseTemplate(ContentType.NewJobPostMatching, new NewJobPostMatchingTemplateModel
                {
                    JobTitle = request.JobTitle,
                    JobCompany = employerCompany!.CompanyName,
                    JobDeadline = request.JobDeadline,
                    DistrictCode = request.DistrictCode,
                    ProvinceCode = request.ProvinceCode
                });
            }
            async Task SendNotificationToJobSeekers()
            {
                List<Task> tasks = [];
                foreach (var jobSeeker in jobSeekers)
                {
                    _logger.LogInformation("Sending notification to job seeker {JobSeekerId} with distance {Distance}",
                    jobSeeker.Item1, jobSeeker.Item2);

                    var notificationDto = await _mediator.Send(new CreateNewNotificationCommand
                    {
                        UserId = jobSeeker.Item1,
                        Message = parsedTemplate,
                        ContentType = ContentType.NewJobPostMatching,
                        JobPostId = request.JobPostId,
                        CreatedAt = DateTime.UtcNow,
                        AdditionalPayload = new Dictionary<string, string> {
                            { "jobPostId", request.JobPostId.ToString() },
                        }
                    }, cancellationToken);

                    var userDevices = await _unitOfWork.Repository<DevicePushNotifications>()
                        .GetAllWithSpecificationAsync(new GetDevicesByUserSpecification(jobSeeker.Item1));

                    tasks.Add(_notificationService.NotifyOneJobSeeker(
                        typeof(INotificationForJobSeekers).GetMethod(nameof(INotificationForJobSeekers.NotifyNewJobPostMatching))!,
                        userDevices.Select(a => a.DeviceToken!)!.ToList(),
                        jobSeeker.Item1.ToString(),
                        notificationDto
                    ));
                }

                await Task.WhenAll(tasks);

                _logger.LogInformation("Sent notification to {Count} job seekers", jobSeekers.Count());
            }
            async Task<IEnumerable<(int, string)>> FilterJobSeekersByDistance()
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

                return jobSeekersWithLocation.Join(
                    filteredDistanceResults, j => j.Item2, d => d.Destination,
                    (j, d) => j);
            }
        }
    }
}
