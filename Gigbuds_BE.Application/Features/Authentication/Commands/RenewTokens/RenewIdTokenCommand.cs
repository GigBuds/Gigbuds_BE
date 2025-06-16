using System;
using MediatR;

namespace Gigbuds_BE.Application.Features.Authentication.Commands.RenewTokens;

public class RenewIdTokenCommand : IRequest<string>
{
    public int userId { get; set; }
}
