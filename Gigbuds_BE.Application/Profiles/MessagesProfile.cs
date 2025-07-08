using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Gigbuds_BE.Application.DTOs.Messages;
using Gigbuds_BE.Domain.Entities.Chats;

namespace Gigbuds_BE.Application.Profiles
{
    public class MessagesProfile : Profile
    {
        public MessagesProfile()
        {
            CreateMap<Message, ChatHistoryDto>()
                .ForMember(dest => dest.MessageId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.ConversationId, opt => opt.MapFrom(src => src.ConversationId))
                .ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.Content))
                .ForMember(dest => dest.SenderName, opt => opt.MapFrom(src => src.Account.FirstName))
                .ForMember(dest => dest.SenderAvatar, opt => opt.MapFrom(src => src.Account.AvatarUrl))
                .ForMember(dest => dest.SenderId, opt => opt.MapFrom(src => src.AccountId))
                .ForMember(dest => dest.Timestamp, opt => opt.MapFrom(src => src.UpdatedAt));

            CreateMap<Conversation, ConversationMetaDataDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()))
                .ForMember(dest => dest.NameOne, opt => opt.MapFrom(src => src.NameOne))
                .ForMember(dest => dest.NameTwo, opt => opt.MapFrom(src => src.NameTwo))
                .ForMember(dest => dest.AvatarOne, opt => opt.MapFrom(src => src.AvatarOne))
                .ForMember(dest => dest.AvatarTwo, opt => opt.MapFrom(src => src.AvatarTwo))
                .ForMember(dest => dest.CreatorId, opt => opt.MapFrom(src => int.Parse(src.CreatorId!)))
                .ForMember(dest => dest.LastMessageSenderName, opt => opt.MapFrom(src => src.LastMessageSenderName))
                .ForMember(dest => dest.LastMessage, opt => opt.MapFrom(src => src.LastMessage))
                .ForMember(dest => dest.Members, opt => opt.MapFrom(src => src.ConversationMembers.Select(cm => new ConversationMemberDto { UserId = cm.AccountId, UserName = cm.Account.FirstName })))
                .ForMember(dest => dest.Timestamp, opt => opt.MapFrom(src => src.UpdatedAt));

        }
    }
}
