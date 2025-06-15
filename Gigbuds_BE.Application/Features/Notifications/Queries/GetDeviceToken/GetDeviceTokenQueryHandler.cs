using Gigbuds_BE.Application.Interfaces.Repositories;
using Gigbuds_BE.Application.Specifications.Notifications;
using Gigbuds_BE.Domain.Entities;
using Gigbuds_BE.Domain.Entities.Notifications;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Gigbuds_BE.Application.Features.Notifications.Queries.GetDeviceToken
{
    internal class GetDeviceTokenQueryHandler : IRequestHandler<GetDeviceTokenQuery, string?>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<GetDeviceTokenQueryHandler> _logger;

        public GetDeviceTokenQueryHandler(IUnitOfWork unitOfWork, ILogger<GetDeviceTokenQueryHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<string?> Handle(GetDeviceTokenQuery request, CancellationToken cancellationToken)
        {
            var deviceToken = await _unitOfWork.Repository<DevicePushNotifications>().GetBySpecificationAsync(new GetDeviceTokenSpecification(request.DeviceId));
            if (deviceToken == null)
            {
                _logger.LogWarning("No device token found for device {DeviceId}", request.DeviceId);
                return null;
            }
            return deviceToken.DeviceToken;
        }
    }
}
