using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using BCrypt.Net;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using PRN232.LMS.Repositories;
using PRN232.LMS.Repositories.Models;
using PRN232.LMS.Services.Models.Response;
using PRN232.LMS.Services.Models.Request;

namespace PRN232.LMS.Services.Services;

public class AuthService(GenericRepository<User> repo, IConfiguration config)
{
    private readonly GenericRepository<User> _repo = repo;
    private readonly IConfiguration _config = config;

    public async Task<TokenResponse?> LoginAsync(string username, string password)
    {
        var user = await _repo.FirstOrDefaultAsync(u => u.Username == username);
        if (user is null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            return null;

        return await IssueTokensAsync(user);
    }

    public async Task<TokenResponse?> RefreshAsync(string refreshToken)
    {
        var user = await _repo.FirstOrDefaultAsync(u =>
            u.RefreshToken == refreshToken && u.RefreshTokenExpiry > DateTime.UtcNow);
        if (user is null) return null;

        return await IssueTokensAsync(user);
    }

    public async Task<bool> RegisterAsync(RegisterRequest request)
    {
        // Check if username already exists
        var existingUser = await _repo.FirstOrDefaultAsync(u => u.Username == request.Username);
        if (existingUser != null)
        {
            return false;
        }

        var user = new User
        {
            Username = request.Username,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
            Role = request.Role
        };

        await _repo.AddAsync(user);
        return true;
    }

    private async Task<TokenResponse> IssueTokensAsync(User user)
    {
        var expiry = int.Parse(_config["Jwt:ExpiresInSeconds"] ?? "3600");
        var accessToken = GenerateJwt(user, expiry);
        var refreshToken = GenerateRefreshToken();

        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);
        await _repo.UpdateAsync(user);

        return new TokenResponse { AccessToken = accessToken, RefreshToken = refreshToken, ExpiresIn = expiry };
    }

    private string GenerateJwt(User user, int expiresInSeconds)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Secret"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Role, user.Role),
        };

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddSeconds(expiresInSeconds),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private static string GenerateRefreshToken()
        => Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
}
