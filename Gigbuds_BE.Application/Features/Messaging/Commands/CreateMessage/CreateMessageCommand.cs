using MediatR;

namespace Gigbuds_BE.Application.Features.Messaging.Commands.CreateMessage
{
    public class CreateMessageCommand : IRequest<int>
    {
        public required int ConversationId { get; set; }
        public required string Content { get; set; }
        public required int SenderId { get; set; }
        public required string SenderName { get; set; }
        public required DateTime SentDate { get; set; }
    }
}
