
using Gigbuds_BE.Application.DTOs.Messages;
using MediatR;

namespace Gigbuds_BE.Application.Features.Messaging.Commands.UpdateMessage
{
    public class UpdateMessageCommand : IRequest
    {
        public required ChatHistoryDto UpdatedEntity { get; set; }
    }
}
