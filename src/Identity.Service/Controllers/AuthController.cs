using Identity.Service.Common;
using Identity.Service.Models.Request;
using Identity.Service.Services;
using Microsoft.AspNetCore.Mvc;
namespace Identity.Service.Controllers;
[ApiController]
[Route("api/auth")]
public class AuthController(AuthService authService) : ControllerBase
{
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var result = await authService.LoginAsync(request);
        return result is null
            ? Unauthorized(ApiResponse<object>.Fail("Invalid credentials"))
            : Ok(ApiResponse<object>.Ok(result));
    }
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        if (!ModelState.IsValid) return BadRequest(ApiResponse<object>.Fail("Validation failed."));
        var success = await authService.RegisterAsync(request);
        return success
            ? Ok(ApiResponse<string>.Ok("User registered successfully."))
            : BadRequest(ApiResponse<object>.Fail("Username already exists."));
    }
    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequest request)
    {
        var result = await authService.RefreshAsync(request.RefreshToken);
        return result is null
            ? Unauthorized(ApiResponse<object>.Fail("Invalid or expired refresh token"))
            : Ok(ApiResponse<object>.Ok(result));
    }
}
