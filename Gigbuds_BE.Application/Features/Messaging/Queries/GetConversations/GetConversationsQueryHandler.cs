
using AutoMapper;
using Gigbuds_BE.Application.DTOs;
using Gigbuds_BE.Application.DTOs.Messages;
using Gigbuds_BE.Application.Features.Messaging.Queries.GetConversations;
using Gigbuds_BE.Application.Interfaces.Repositories;
using Gigbuds_BE.Application.Interfaces.Services.MessagingServices;
using Gigbuds_BE.Application.Specifications.Messaging;
using Gigbuds_BE.Domain.Entities.Chats;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Gigbuds_BE.Application.Features.Messaging.Queries
{
    internal class GetConversationsQueryHandler : IRequestHandler<GetConversationsQuery, PagedResultDto<ConversationMetaDataDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<GetConversationsQueryHandler> _logger;
        private readonly IMessagingCacheService _messageCacheService;

        public GetConversationsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<GetConversationsQueryHandler> logger, IMessagingCacheService messageCacheService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
            _messageCacheService = messageCacheService;
        }

        public async Task<PagedResultDto<ConversationMetaDataDto>> Handle(GetConversationsQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling GetConversationsQuery for user {UserId} with params: {@QueryParams}", request.QueryParams.UserId, request.QueryParams);

            var cacheResult = await _messageCacheService.GetConversationsAsync(
                request.QueryParams
            );

            if (cacheResult != null && cacheResult.Count > 0)
            {
                return new PagedResultDto<ConversationMetaDataDto>(cacheResult.Count, cacheResult);
            }

            var conversations = await _messageCacheService.RetrieveConversationsFromServerAsync(request.QueryParams);

            return new PagedResultDto<ConversationMetaDataDto>(conversations.Count, conversations);
        }
    }
}
