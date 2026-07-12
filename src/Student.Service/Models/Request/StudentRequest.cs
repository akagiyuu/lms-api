using System.ComponentModel.DataAnnotations;
namespace Student.Service.Models.Request;
public class CreateStudentRequest
{
    [Required, MaxLength(100)]             public string FullName    { get; set; } = null!;
    [Required, MaxLength(100), EmailAddress] public string Email    { get; set; } = null!;
    [Required]                             public DateTimeOffset? DateOfBirth { get; set; }
}
public class UpdateStudentRequest
{
    [MaxLength(100)]             public string? FullName    { get; set; }
    [MaxLength(100), EmailAddress] public string? Email    { get; set; }
    public DateTimeOffset? DateOfBirth { get; set; }
}
