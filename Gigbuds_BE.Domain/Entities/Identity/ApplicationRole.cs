using System;
using Microsoft.AspNetCore.Identity;

namespace Gigbuds_BE.Domain.Entities.Identity;

public class ApplicationRole : IdentityRole<int>
{
    // Constructor for empty initialization
    public ApplicationRole() : base() { }
    
    // Constructor with role name
    public ApplicationRole(string roleName) : base(roleName) { }
}
