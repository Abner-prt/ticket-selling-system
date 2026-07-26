using Microsoft.AspNetCore.Identity;

namespace ticket_selling_backend.Entities;

// Extiende IdentityUser para agregar campos personalizados
public class UserEntity : IdentityUser
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? AvatarUrl { get; set; }

    // Token para renovar el JWT sin pedir credenciales
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpireTime { get; set; }
}
