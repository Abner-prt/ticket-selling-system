namespace ticket_selling_backend.DTOs;

public class TokenDto
{
    public string AccessToken { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
    // Tiempo en minutos hasta que el access token expira
    public int ExpiresIn { get; set; }
}
