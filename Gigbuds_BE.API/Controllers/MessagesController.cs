using Gigbuds_BE.Application.Features.Messaging.Queries;
using Gigbuds_BE.Application.Features.Messaging.Queries.GetConversations;
using Gigbuds_BE.Application.Features.Messaging.Queries.GetMessages;
using Gigbuds_BE.Application.Specifications.Messaging;
using Gigbuds_BE.Infrastructure.Services.SignalR;
using Gigbuds_BE.Application.Interfaces.Services.MessagingServices;
using Gigbuds_BE.Application.DTOs.Messages;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace Gigbuds_BE.API.Controllers
{
    public class MessagesController : _BaseApiController
    {
        private readonly IMediator _mediator;
        private readonly IHubContext<MessagingHub, IMessagingClient> _hubContext;

        public MessagesController(IMediator mediator, IHubContext<MessagingHub, IMessagingClient> hubContext)
        {
            _mediator = mediator;
            _hubContext = hubContext;
        }

        [HttpGet("conversation-metadata")]
        public async Task<IActionResult> GetConversationsWithMessages([FromQuery] ConversationQueryParams queryParams)
        {
            var result = await _mediator.Send(new GetConversationsQuery(queryParams));

            return Ok(result);
        }

        [HttpGet("conversation-messages")]
        public async Task<IActionResult> GetConversationMessages([FromQuery] MessagesQueryParams queryParams)
        {
            var result = await _mediator.Send(new GetMessagesQuery(queryParams));

            return Ok(result);
        }

        [HttpPost("send-typing-indicator")]
        public async Task<IActionResult> SendTypingIndicator([FromBody] TypingIndicatorRequest request)
        {
            await _hubContext.Clients.Group(request.ConversationId.ToString())
                .ReceiveTypingIndicatorAsync(request.IsTyping, request.TyperName);

            return Ok(new { Message = "Typing indicator sent successfully" });
        }

        [HttpPost("broadcast-to-conversation")]
        public async Task<IActionResult> BroadcastToConversation([FromBody] BroadcastRequest request)
        {
            await _hubContext.Clients.User(request.UserId.ToString())
                .ReceiveMessageAsync(request.Conversation, request.Message);


            return Ok(new { Message = "Message broadcasted successfully" });
        }
    }

    public class TypingIndicatorRequest
    {
        public string TyperName { get; set; }
        public int ConversationId { get; set; }
        public bool IsTyping { get; set; }
    }

    public class BroadcastRequest
    {
        public int UserId { get; set; }
        public ConversationMetaDataDto Conversation { get; set; }
        public ChatHistoryDto Message { get; set; }
        // public string MethodName { get; set; } = string.Empty;
        // public object Data { get; set; } = new();
    }
}
