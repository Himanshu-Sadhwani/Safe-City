using System;

namespace SafeCity.DTOs;

public class ViewAllUsersResponseDto
{
    public int UserId { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string Status { get; set; }
    public string RoleName { get; set; }
}
