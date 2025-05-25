using System;

namespace Gigbuds_BE.Domain.Entities.Constants;

public static class UserRoles
{
    public const string JobSeeker = "JobSeeker";
    public const string Employer = "Employer";
    public const string Staff = "Staff";
    public const string Admin = "Admin";
    public static readonly Dictionary<string, string> RoleMap = new()
    {
        { "1", JobSeeker },
        { "2", Employer },
        { "3", Staff },
        { "4", Admin }
    };
}
