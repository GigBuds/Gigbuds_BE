using Gigbuds_BE.Application.DTOs.Messages;
using MediatR;

namespace Gigbuds_BE.Application.Features.Messaging.Commands.CreateConversation
{
    public class CreateConversationCommand : IRequest<ConversationMetaDataDto>
    {
        /// <summary>
        /// List of members with their user id and display name
        /// </summary>
        /// 
        public required Dictionary<int, string> Members { get; set; }
        public required string CreatorId { get; set; }
        public required string ConversationNameOne { get; set; }
        public required string ConversationNameTwo { get; set; }
        public required string AvatarOne { get; set; }
        public required string AvatarTwo { get; set; }
        public required DateTime CreatedAt { get; set; }
    }
}
