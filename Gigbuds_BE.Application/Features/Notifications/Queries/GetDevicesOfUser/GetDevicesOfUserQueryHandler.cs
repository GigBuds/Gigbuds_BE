
using AutoMapper;
using Gigbuds_BE.Application.DTOs.Notifications;
using Gigbuds_BE.Application.Interfaces.Repositories;
using Gigbuds_BE.Application.Specifications.Notifications;
using Gigbuds_BE.Domain.Entities.Notifications;
using MediatR;

namespace Gigbuds_BE.Application.Features.Notifications.Queries.GetDevicesOfUser
{
    internal class GetDevicesOfUserQueryHandler : IRequestHandler<GetDevicesOfUserQuery, List<UserDeviceDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetDevicesOfUserQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<UserDeviceDto>> Handle(GetDevicesOfUserQuery request, CancellationToken cancellationToken)
        {
            var devices = await _unitOfWork.Repository<DevicePushNotifications>()
                .GetAllWithSpecificationProjectedAsync<UserDeviceDto>(new GetDevicesByUserSpecification(request.UserId), _mapper.ConfigurationProvider);
            return devices.ToList();
        }
    }
}
