using System;
using MediatR;

namespace Gigbuds_BE.Application.Features.Accounts;

public class BanAccountCommand : IRequest<bool>
{
    public int Id { get; set; }
    public bool IsEnabled { get; set; }

    public BanAccountCommand(int id, bool isEnabled)
    {
        Id = id;
        IsEnabled = isEnabled;
    }
}
