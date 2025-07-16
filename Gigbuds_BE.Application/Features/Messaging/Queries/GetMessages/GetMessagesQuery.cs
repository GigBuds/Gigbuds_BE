
using Gigbuds_BE.Application.DTOs;
using Gigbuds_BE.Application.DTOs.Messages;
using Gigbuds_BE.Application.Specifications.Messaging;
using MediatR;

namespace Gigbuds_BE.Application.Features.Messaging.Queries.GetMessages
{
    public class GetMessagesQuery(MessagesQueryParams queryParams) : IRequest<PagedResultDto<ChatHistoryDto>>
    {
        public MessagesQueryParams QueryParams { get; set; } = queryParams;
    }
}
