using Models.Entities;

namespace Models.Dto;

public class AuthorizationResponse
{
    public int Id { get; set; }
    public string Email { get; set; }
    public string LastName { get; set; }
    public string FirstName { get; set; }
    public string? MiddleName { get; set; }
    public byte[]? Image { get; set; }
    public string Token { get; set; }
    public RoleDto Role { get; set; }
    
}