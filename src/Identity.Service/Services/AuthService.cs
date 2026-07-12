using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Identity.Service.Models;
using Identity.Service.Models.Request;
using Identity.Service.Models.Response;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
namespace Identity.Service.Services;
public class AuthService(AppDbContext db, IConfiguration config)
{
    public async Task<TokenResponse?> LoginAsync(LoginRequest request)
    {
        var user = await db.Users.FirstOrDefaultAsync(u => u.Username == request.Username);
        if (user is null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash)) return null;
        return await IssueTokensAsync(user);
    }
    public async Task<TokenResponse?> RefreshAsync(string refreshToken)
    {
        var user = await db.Users.FirstOrDefaultAsync(u =>
            u.RefreshToken == refreshToken && u.RefreshTokenExpiry > DateTime.UtcNow);
        if (user is null) return null;
        return await IssueTokensAsync(user);
    }
    public async Task<bool> RegisterAsync(RegisterRequest request)
    {
        if (await db.Users.AnyAsync(u => u.Username == request.Username)) return false;
        var user = new User
        {
            Username     = request.Username,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
            Role         = request.Role
        };
        db.Users.Add(user);
        await db.SaveChangesAsync();
        return true;
    }
    private async Task<TokenResponse> IssueTokensAsync(User user)
    {
        var expiry       = int.Parse(config["Jwt:ExpiresInSeconds"] ?? "3600");
        var accessToken  = GenerateJwt(user, expiry);
        var refreshToken = GenerateRefreshToken();
        user.RefreshToken       = refreshToken;
        user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);
        await db.SaveChangesAsync();
        return new TokenResponse { AccessToken = accessToken, RefreshToken = refreshToken, ExpiresIn = expiry };
    }
    private string GenerateJwt(User user, int expiresInSeconds)
    {
        var key   = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Secret"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
            new Claim(ClaimTypes.Name,           user.Username),
            new Claim(ClaimTypes.Role,           user.Role),
        };
        var token = new JwtSecurityToken(
            issuer: config["Jwt:Issuer"], audience: config["Jwt:Audience"],
            claims: claims, expires: DateTime.UtcNow.AddSeconds(expiresInSeconds),
            signingCredentials: creds);
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
    private static string GenerateRefreshToken() => Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
}
