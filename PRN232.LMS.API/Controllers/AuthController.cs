using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using PRN232.LMS.API.Common;
using PRN232.LMS.Services.Models.Request;
using PRN232.LMS.Services.Models.Response;
using PRN232.LMS.Services.Services;

namespace PRN232.LMS.API.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/auth")]
[ProducesResponseType(StatusCodes.Status500InternalServerError)]
public class AuthController(AuthService authService) : ControllerBase
{
    private readonly AuthService _authService = authService;

    /// <summary>Authenticate and receive a JWT access token.</summary>
    [HttpPost("login")]
    [ProducesResponseType(typeof(ApiResponse<TokenResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<ApiResponse<TokenResponse>>> Login([FromBody] LoginRequest request)
    {
        var token = await _authService.LoginAsync(request.Username!, request.Password!);
        return token is null
            ? Unauthorized(ApiResponse<TokenResponse>.Fail("Invalid username or password."))
            : Ok(ApiResponse<TokenResponse>.Ok(token));
    }

    /// <summary>Exchange a refresh token for a new access token.</summary>
    [HttpPost("refresh-token")]
    [ProducesResponseType(typeof(ApiResponse<TokenResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<ApiResponse<TokenResponse>>> RefreshToken([FromBody] RefreshTokenRequest request)
    {
        var token = await _authService.RefreshAsync(request.RefreshToken!);
        return token is null
            ? Unauthorized(ApiResponse<TokenResponse>.Fail("Invalid or expired refresh token."))
            : Ok(ApiResponse<TokenResponse>.Ok(token));
    }

    /// <summary>Register a new user.</summary>
    [HttpPost("register")]
    [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse<string>>> Register([FromBody] RegisterRequest request)
    {
        var success = await _authService.RegisterAsync(request);
        if (!success)
        {
            return BadRequest(ApiResponse<string>.Fail("Username already exists."));
        }

        return StatusCode(201, ApiResponse<string>.Ok("User registered successfully."));
    }
}
