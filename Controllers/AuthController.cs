using Microsoft.AspNetCore.Mvc;
using ticket_selling_backend.DTOs;
using ticket_selling_backend.Services;

namespace ticket_selling_backend.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto dto)
    {
        var (success, user, token, error) = await _authService.RegisterAsync(dto);

        if (!success)
            return BadRequest(new { statusCode = 400, message = error, status = "error", data = (object?)null });

        return Ok(new { statusCode = 200, message = "Registro exitoso", status = "success", data = new { user, token } });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        var (success, user, token, error) = await _authService.LoginAsync(dto);

        if (!success)
            return Unauthorized(new { statusCode = 401, message = error, status = "error", data = (object?)null });

        return Ok(new { statusCode = 200, message = "Login exitoso", status = "success", data = new { user, token } });
    }

    // Renueva el access token usando el refresh token
    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenDto dto)
    {
        var (success, token, error) = await _authService.RefreshTokenAsync(dto.RefreshToken);

        if (!success)
            return Unauthorized(new { statusCode = 401, message = error, status = "error", data = (object?)null });

        return Ok(new { statusCode = 200, message = "Token renovado", status = "success", data = token });
    }
}
