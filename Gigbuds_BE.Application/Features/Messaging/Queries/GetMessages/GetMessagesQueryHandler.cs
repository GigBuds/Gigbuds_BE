using AutoMapper;
using Gigbuds_BE.Application.DTOs;
using Gigbuds_BE.Application.DTOs.Messages;
using Gigbuds_BE.Application.Interfaces.Repositories;
using Gigbuds_BE.Application.Interfaces.Services.MessagingServices;
using Gigbuds_BE.Domain.Entities.Chats;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Gigbuds_BE.Application.Features.Messaging.Queries.GetMessages
{
    internal class GetMessagesQueryHandler : IRequestHandler<GetMessagesQuery, PagedResultDto<ChatHistoryDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<GetMessagesQueryHandler> _logger;
        private readonly IMessagingCacheService _messageCacheService;

        public GetMessagesQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<GetMessagesQueryHandler> logger, IMessagingCacheService messageCacheService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
            _messageCacheService = messageCacheService;
        }

        public async Task<PagedResultDto<ChatHistoryDto>> Handle(GetMessagesQuery request, CancellationToken cancellationToken)
        {
            var cacheResult = await _messageCacheService.GetMessagesAsync(
                request.QueryParams
            );
            if (cacheResult != null && cacheResult.Count > 0)
            {
                return new PagedResultDto<ChatHistoryDto>(cacheResult.Count, cacheResult);
            }

            var messages = await _messageCacheService.RetrieveMessagesFromServerAsync(request.QueryParams);

            _logger.LogInformation("Fetched {Count} messages for conversation {ConversationId}", messages.Count, request.QueryParams.ConversationId);
            return new PagedResultDto<ChatHistoryDto>(messages.Count, messages);
        }
    }
}
