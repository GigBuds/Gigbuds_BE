using System;
using System.Text.Json.Serialization;
using MediatR;

namespace Gigbuds_BE.Application.Features.Memberships.Commands.UpdateMembershipEntity;

public class UpdateMembershipCommand : IRequest<bool>
{
    [JsonIgnore]
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public int Duration { get; set; }
}
