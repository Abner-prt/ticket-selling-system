using ticket_selling_backend.DTOs;

namespace ticket_selling_backend.Services;

public interface IAuthService
{
    Task<(bool Success, UserDto? User, TokenDto? Token, string Error)> RegisterAsync(RegisterDto dto);
    Task<(bool Success, UserDto? User, TokenDto? Token, string Error)> LoginAsync(LoginDto dto);
    Task<(bool Success, TokenDto? Token, string Error)> RefreshTokenAsync(string refreshToken);
}
