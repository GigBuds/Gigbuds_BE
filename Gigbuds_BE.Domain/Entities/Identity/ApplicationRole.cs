using System;
using Microsoft.AspNetCore.Identity;

namespace Gigbuds_BE.Domain.Entities.Identity;

public class ApplicationRole : IdentityRole<int>
{
    public ApplicationRole() : base() { }
       
    public ApplicationRole(string roleName) : base(roleName)
    {
    }
}
