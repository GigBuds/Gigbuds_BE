using System;
using MediatR;

namespace Gigbuds_BE.Application.Features.Authentication.Commands.Register.RegisterForAdmin;

public class RegisterForAdminCommand : IRequest
{
    public DateTime Dob { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? Email { get; set; }
    public string Password { get; set; }
    public string PhoneNumber { get; set; }
    public bool IsMale { get; set; } = true;
} 