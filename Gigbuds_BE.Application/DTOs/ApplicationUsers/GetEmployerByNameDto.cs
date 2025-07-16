using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gigbuds_BE.Application.DTOs.ApplicationUsers
{
    public class GetEmployerByNameDto
    {
        public int UserId { get; set; }
        public required string FullName { get; set; } = string.Empty;
        public required string Avatar { get; set; } = string.Empty;

    }
}