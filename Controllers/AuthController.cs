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
            return BadRequest(ResponseDto<object>.Failure(error ?? "Error en registro", 400));

        return Ok(ResponseDto<object>.Success(new { user, token }, "Registro exitoso", 200));
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        var (success, user, token, error) = await _authService.LoginAsync(dto);

        if (!success)
            return Unauthorized(ResponseDto<object>.Failure(error ?? "Credenciales inválidas", 401));

        return Ok(ResponseDto<object>.Success(new { user, token }, "Login exitoso", 200));
    }

    // Renueva el access token usando el refresh token
    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenDto dto)
    {
        var (success, token, error) = await _authService.RefreshTokenAsync(dto.RefreshToken);

        if (!success)
            return Unauthorized(ResponseDto<object>.Failure(error ?? "Token inválido", 401));

        return Ok(ResponseDto<TokenDto>.Success(token!, "Token renovado", 200));
    }
}
