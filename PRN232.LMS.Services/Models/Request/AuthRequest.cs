using System.ComponentModel.DataAnnotations;

namespace PRN232.LMS.Services.Models.Request;

public class LoginRequest
{
    [Required] public string? Username { get; set; }
    [Required] public string? Password { get; set; }
}

public class RefreshTokenRequest
{
    [Required] public string? RefreshToken { get; set; }
}
