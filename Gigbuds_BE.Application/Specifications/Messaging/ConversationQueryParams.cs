namespace Gigbuds_BE.Application.Specifications.Messaging
{
    public class ConversationQueryParams : BasePagingParams
    {
        // Id of the user making the request
        public int UserId { get; set; } = -1;

        // Metadata for conversation search
        public string? SearchTerm { get; set; } = null;
    }
}
