using System;

namespace Gigbuds_BE.Application.DTOs.Followers;

public class FollowerResponseDto
{
    public int FollowerAccountId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string AvatarUrl { get; set; }

}
