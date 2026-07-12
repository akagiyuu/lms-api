using System.ComponentModel.DataAnnotations;
namespace Identity.Service.Models.Request;
public class LoginRequest
{
    [Required] public string Username { get; set; } = null!;
    [Required] public string Password { get; set; } = null!;
}
public class RegisterRequest
{
    [Required, MinLength(3), MaxLength(50)] public string Username { get; set; } = null!;
    [Required, MinLength(6)]                public string Password { get; set; } = null!;
    [Required]                              public string FullName  { get; set; } = null!;
    [Required]                              public string Role      { get; set; } = "Student";
}
public class RefreshTokenRequest
{
    [Required] public string RefreshToken { get; set; } = null!;
}
