using SDS.Domain.Entities;

namespace SDS.Application.DTOs;

public class UserDto
{
    public Guid Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? DisplayName { get; set; }
    public bool IsActive { get; set; }
    public List<RoleType> Roles { get; set; } = new();
}

public class InviteUserRequest
{
    public string Email { get; set; } = string.Empty;
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public List<RoleType> Roles { get; set; } = new();
}
