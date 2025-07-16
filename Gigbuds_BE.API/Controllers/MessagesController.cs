using Gigbuds_BE.Application.DTOs.Messages;
using Gigbuds_BE.Application.Features.Messaging.Commands.CreateConversation;
using Gigbuds_BE.Application.Features.Messaging.Commands.CreateMessage;
using Gigbuds_BE.Application.Features.Messaging.Commands.DeleteMessage;
using Gigbuds_BE.Application.Features.Messaging.Commands.UpdateMessage;
using Gigbuds_BE.Application.Features.Messaging.Queries;
using Gigbuds_BE.Application.Features.Messaging.Queries.GetConversations;
using Gigbuds_BE.Application.Features.Messaging.Queries.GetMessages;
using Gigbuds_BE.Application.Interfaces.Services.MessagingServices;
using Gigbuds_BE.Application.Specifications.Messaging;
using Gigbuds_BE.Infrastructure.Services.SignalR;
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

        [HttpPost("conversation-metadata")]
        public async Task<IActionResult> CreateConversation([FromBody] CreateConversationCommand command)
        {
            var result = await _mediator.Send(command);

            return Ok(result);
        }

        [HttpGet("conversation-messages")]
        public async Task<IActionResult> GetConversationMessages([FromQuery] MessagesQueryParams queryParams)
        {
            var result = await _mediator.Send(new GetMessagesQuery(queryParams));

            return Ok(result);
        }

        [HttpPut("conversation-messages")]
        public async Task<IActionResult> UpdateConversationMessage([FromBody] ChatHistoryDto message)
        {
            await _mediator.Send(new UpdateMessageCommand
            {
                UpdatedEntity = message,
            });

            return Ok();
        }

        [HttpDelete("conversation-messages/{messageId}")]
        public async Task<IActionResult> DeleteConversationMessage([FromRoute] int messageId)
        {
            var result = await _mediator.Send(new DeleteMessageCommand
            {
                MessageId = messageId,
            });

            return Ok(result);
        }
    }
}
