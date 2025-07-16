using Gigbuds_BE.Application.DTOs;
using Gigbuds_BE.Application.DTOs.Messages;
using Gigbuds_BE.Application.Specifications.Messaging;
using MediatR;

namespace Gigbuds_BE.Application.Features.Messaging.Queries.GetConversations
{
    public class GetConversationsQuery(ConversationQueryParams conversationQueryParams) : IRequest<PagedResultDto<ConversationMetaDataDto>>
    {
        public ConversationQueryParams QueryParams { get; set; } = conversationQueryParams;
    }
}