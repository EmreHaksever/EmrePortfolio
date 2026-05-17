using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Portfolio.Business.Abstract;
using Portfolio.Business.DTOs;
using Portfolio.DataAccess.Contexts;

namespace Portfolio.Business.Concrete;

public class AuthManager : IAuthService
{
    private readonly PortfolioDbContext _context;
    private readonly IConfiguration _configuration;

    public AuthManager(PortfolioDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    public async Task<string> LoginAsync(LoginDto loginDto)
    {
        // 1. Kullanıcıyı veritabanında ara
        var user = await _context.AppUsers.FirstOrDefaultAsync(u =>
            u.Username == loginDto.Username && u.Password == loginDto.Password);

        if (user == null) return null; // Kullanıcı bulunamazsa null dön

        // 2. JWT Ayarlarını Hazırla
        var jwtSettings = _configuration.GetSection("Jwt");
        var jwtKey = Environment.GetEnvironmentVariable("JWT_KEY") ?? jwtSettings["Key"];
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        // 3. Token İçindeki Bilgiler (Claims)
        var claims = new[]
        {
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
        };

        // 4. Token Oluşturma
        var token = new JwtSecurityToken(
            issuer: jwtSettings["Issuer"],
            audience: jwtSettings["Audience"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(double.Parse(jwtSettings["ExpireMinutes"])),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}