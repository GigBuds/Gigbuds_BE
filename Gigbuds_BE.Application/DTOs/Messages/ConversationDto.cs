using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Redis.OM.Modeling;

namespace Gigbuds_BE.Application.DTOs.Messages
{
    public class ConversationDto
    {
        public int Id { get; set; }
        public string? NameOne { get; set; }
        public string? NameTwo { get; set; }
        public List<int> Members { get; set; } = new List<int>();
    }

    public class ConversationMemberDto
    {
        [Indexed]
        public int UserId { get; set; }
        [Indexed]
        public required string UserName { get; set; }
    }

    [Document(StorageType = StorageType.Json, Prefixes = new[] { "ConversationMetaData" })]
    public class ConversationMetaDataDto
    {
        [RedisIdField]
        [Indexed]
        public required string Id { get; set; } //use string since redis OM does not support int as Id
        public int CreatorId { get; set; }

        [Searchable]
        public required string NameOne { get; set; }
        [Searchable]
        public required string NameTwo { get; set; }
        public required string AvatarOne { get; set; }
        public required string AvatarTwo { get; set; }
        public required string LastMessage { get; set; }
        public required string LastMessageSenderName { get; set; }
        public required string LastMessageId { get; set; } = string.Empty;

        [Indexed(Sortable = true)]
        public DateTime Timestamp { get; set; } = DateTime.Now;
        public bool IsOnline { get; set; } = false;

        [Indexed(CascadeDepth = 1)]
        public ConversationMemberDto[] WhosTyping { get; set; } = [];
        [Indexed]
        public string[] MemberIds { get; set; } = [];
        [Indexed(CascadeDepth = 1)]
        public ConversationMemberDto[] Members { get; set; } = [];
    }

    public enum ConversationRole
    {
        Member = 0,
        Admin = 1,
        Owner = 2
    }
}