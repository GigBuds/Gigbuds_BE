using Gigbuds_BE.Domain.Entities.Chats;

namespace Gigbuds_BE.Application.Specifications.Messaging
{
    public class GetMessagesSpecification : BaseSpecification<Message>
    {
        public GetMessagesSpecification(MessagesQueryParams messagesQueryParams)
            : base(x =>
                x.ConversationId == messagesQueryParams.ConversationId
                    && (string.IsNullOrEmpty(messagesQueryParams.SearchTerm)
                        || x.Content.ToLower().Contains(messagesQueryParams.SearchTerm.ToLower()))
            )
        {
            AddPaging(messagesQueryParams.PageSize * (messagesQueryParams.PageIndex - 1), messagesQueryParams.PageSize);
            AddOrderByDesc(x => x.SendDate); // order from oldest to newest since client renders from top to bottom
        }
    }
}
