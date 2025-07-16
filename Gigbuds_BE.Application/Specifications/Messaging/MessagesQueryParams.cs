namespace Gigbuds_BE.Application.Specifications.Messaging
{
    public class MessagesQueryParams : BasePagingParams
    {
        public int ConversationId { get; set; }
        public string? SearchTerm { get; set; } = null;
    }
}
