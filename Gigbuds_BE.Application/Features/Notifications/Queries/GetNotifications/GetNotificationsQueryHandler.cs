using AutoMapper;
using Gigbuds_BE.Application.DTOs;
using Gigbuds_BE.Application.DTOs.Notifications;
using Gigbuds_BE.Application.Interfaces.Repositories;
using Gigbuds_BE.Application.Specifications.Notifications;
using Gigbuds_BE.Domain.Entities.Notifications;
using MediatR;

namespace Gigbuds_BE.Application.Features.Notifications.Queries.GetNotifications
{
    internal class GetNotificationsQueryHandler : IRequestHandler<GetNotificationsQuery, PagedResultDto<NotificationDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetNotificationsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PagedResultDto<NotificationDto>> Handle(GetNotificationsQuery request, CancellationToken cancellationToken)
        {
            var spec = new GetNotificationsQuerySpecification(request.Params);
            var notificationsCount = await _unitOfWork.Repository<Notification>().CountAsync(spec);
            var notifications = await _unitOfWork.Repository<Notification>()
            .GetAllWithSpecificationProjectedAsync<NotificationDto>(spec, _mapper.ConfigurationProvider);

            return new PagedResultDto<NotificationDto>(notificationsCount, notifications);
        }
    }
}
