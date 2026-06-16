using System.ComponentModel.DataAnnotations;

namespace PRN232.LMS.Services.Models.Request;

public class RegisterRequest
{
    [Required]
    [StringLength(50, MinimumLength = 3)]
    public string Username { get; set; } = string.Empty;

    [Required]
    [StringLength(100, MinimumLength = 6)]
    public string Password { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string FullName { get; set; } = string.Empty;

    public string Role { get; set; } = "Student"; // Default role
}
