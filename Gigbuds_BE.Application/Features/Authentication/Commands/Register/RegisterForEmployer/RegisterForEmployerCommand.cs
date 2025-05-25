using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gigbuds_BE.Application.Features.Authentication.Commands.Register.RegisterForEmployer
{
    public class RegisterForEmployerCommand : IRequest
    {
        public DateTime Dob { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? Email { get; set; }
        public string Password { get; set; }
        public string SocialSecurityNumber { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsMale { get; set; } = true;
        public string BusinessEmail { get; set; }
    }
}
