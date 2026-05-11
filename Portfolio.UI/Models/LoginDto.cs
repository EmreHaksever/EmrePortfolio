namespace Portfolio.UI.Models;

public class LoginDto
{
    public string Username { get; set; }
    public string Password { get; set; }
}

// API'den dönecek olan JSON yapısını karşılamak için küçük bir sınıf
public class TokenResponse
{
    public string Token { get; set; }
}