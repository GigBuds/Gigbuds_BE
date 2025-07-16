using MediatR;

namespace Gigbuds_BE.Application.Features.Messaging.Commands.DeleteMessage
{
    public class DeleteMessageCommand : IRequest<int>
    {
        public required int MessageId { get; set; }
    }
}
