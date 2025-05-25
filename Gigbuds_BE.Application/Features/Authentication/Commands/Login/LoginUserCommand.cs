using System;
using Gigbuds_BE.Application.DTOs.ApplicationUsers;
using MediatR;

namespace Gigbuds_BE.Application.Features.Authentication.Commands.Login;

public class LoginUserCommand : IRequest<LoginResponseDTO>
{
    public string Identifier { get; set; }
    public string Password { get; set; }
}
