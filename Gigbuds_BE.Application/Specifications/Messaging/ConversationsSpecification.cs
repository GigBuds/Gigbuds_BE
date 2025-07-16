

using Gigbuds_BE.Domain.Entities.Chats;

namespace Gigbuds_BE.Application.Specifications.Messaging
{
    public class GetConversationsSpecification : BaseSpecification<Conversation>
    {
        public GetConversationsSpecification(ConversationQueryParams conversationQueryParams)
            : base(x =>
                (conversationQueryParams.UserId == -1 || x.ConversationMembers.Any(cm => cm.AccountId == conversationQueryParams.UserId))
                && (string.IsNullOrEmpty(conversationQueryParams.SearchTerm) ||
                    x.NameOne.ToLower().Contains(conversationQueryParams.SearchTerm.ToLower()))
            )
        {
            AddPaging(conversationQueryParams.PageSize * (conversationQueryParams.PageIndex - 1), conversationQueryParams.PageSize);
            AddInclude(x => x.ConversationMembers);
            AddOrderBy(x => x.UpdatedAt);
        }

    }

    public class GetConversationByIdSpecification : BaseSpecification<Conversation>
    {
        public GetConversationByIdSpecification(int conversationId) : base(x => x.Id == conversationId)
        {
            // weird af bug, adding this line will cause error c0.Id does not exist; c0 here is the conversation member
            // AddInclude(x => x.ConversationMembers);
        }
    }
}
