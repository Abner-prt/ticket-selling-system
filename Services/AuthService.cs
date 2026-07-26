using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using ticket_selling_backend.DTOs;
using ticket_selling_backend.Entities;

namespace ticket_selling_backend.Services;

public class AuthService : IAuthService
{
    private readonly UserManager<UserEntity> _userManager;
    private readonly IConfiguration _config;

    public AuthService(UserManager<UserEntity> userManager, IConfiguration config)
    {
        _userManager = userManager;
        _config = config;
    }

    public async Task<(bool Success, UserDto? User, TokenDto? Token, string Error)> RegisterAsync(RegisterDto dto)
    {
        var existing = await _userManager.FindByEmailAsync(dto.Email);
        if (existing != null)
            return (false, null, null, "El correo ya esta registrado");

        var user = new UserEntity
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Email = dto.Email,
            UserName = dto.Email
        };

        var result = await _userManager.CreateAsync(user, dto.Password);
        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            return (false, null, null, errors);
        }

        // Asigna rol User por defecto
        await _userManager.AddToRoleAsync(user, "User");

        var roles = await _userManager.GetRolesAsync(user);
        var token = await GenerateTokensAsync(user, roles);
        var userDto = MapToDto(user, roles);

        return (true, userDto, token, string.Empty);
    }

    public async Task<(bool Success, UserDto? User, TokenDto? Token, string Error)> LoginAsync(LoginDto dto)
    {
        var user = await _userManager.FindByEmailAsync(dto.Email);
        if (user == null)
            return (false, null, null, "Credenciales invalidas");

        var validPassword = await _userManager.CheckPasswordAsync(user, dto.Password);
        if (!validPassword)
            return (false, null, null, "Credenciales invalidas");

        var roles = await _userManager.GetRolesAsync(user);
        var token = await GenerateTokensAsync(user, roles);
        var userDto = MapToDto(user, roles);

        return (true, userDto, token, string.Empty);
    }

    public async Task<(bool Success, TokenDto? Token, string Error)> RefreshTokenAsync(string refreshToken)
    {
        // Busca al usuario dueno del refresh token
        var user = _userManager.Users
            .FirstOrDefault(u => u.RefreshToken == refreshToken);

        if (user == null || user.RefreshTokenExpireTime < DateTime.UtcNow)
            return (false, null, "Refresh token invalido o expirado");

        var roles = await _userManager.GetRolesAsync(user);
        var token = await GenerateTokensAsync(user, roles);

        return (true, token, string.Empty);
    }

    // Genera el JWT y el refresh token, y los persiste en el usuario
    private async Task<TokenDto> GenerateTokensAsync(UserEntity user, IList<string> roles)
    {
        var jwtSettings = _config.GetSection("JWT");
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Secret"]!));
        var expirationMinutes = int.Parse(jwtSettings["ExpirationMinutes"]!);
        var refreshDays = int.Parse(jwtSettings["RefreshTokenExpirationDays"]!);

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id),
            new(JwtRegisteredClaimNames.Email, user.Email!),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(ClaimTypes.Name, $"{user.FirstName} {user.LastName}")
        };

        // Agrega cada rol como claim
        foreach (var role in roles)
            claims.Add(new Claim(ClaimTypes.Role, role));

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(expirationMinutes),
            Issuer = jwtSettings["Issuer"],
            Audience = jwtSettings["Audience"],
            SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
        };

        var handler = new JwtSecurityTokenHandler();
        var jwt = handler.CreateToken(tokenDescriptor);

        // Genera un refresh token aleatorio seguro
        var newRefreshToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));

        user.RefreshToken = newRefreshToken;
        user.RefreshTokenExpireTime = DateTime.UtcNow.AddDays(refreshDays);
        await _userManager.UpdateAsync(user);

        return new TokenDto
        {
            AccessToken = handler.WriteToken(jwt),
            RefreshToken = newRefreshToken,
            ExpiresIn = expirationMinutes
        };
    }

    private static UserDto MapToDto(UserEntity user, IList<string> roles) => new()
    {
        Id = user.Id,
        FirstName = user.FirstName,
        LastName = user.LastName,
        Email = user.Email!,
        AvatarUrl = user.AvatarUrl,
        Roles = roles
    };
}
