using Models.Dto;
using Models.Entities;

namespace DesktopApp.Services;

public class AuthService
{
    internal int Id { get; set; }
    internal string Email { get; set; }
    internal RoleDto Role { get; set; }
    internal string LastName { get; set; }
    internal string FirstName { get; set; }
    internal string? MiddleName { get; set; }
    internal byte[]? Image { get; set; }
    public void PutUserData(AuthorizationResponse response)
    {
        Email = response.Email;
        Id = response.Id;
        Role = response.Role;
        LastName = response.LastName;
        MiddleName = response.MiddleName;
        FirstName = response.FirstName;
        Image = response.Image;
    }
    
    public void ClearUserData()
    {
        Email = null;
        Id = 0;
        Role = null;
        LastName = null;
        MiddleName = null;
        FirstName = null;
        Image = null;
    }
}