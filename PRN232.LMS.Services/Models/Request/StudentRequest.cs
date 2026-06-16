using System.ComponentModel.DataAnnotations;

namespace PRN232.LMS.Services.Models.Request;

public class CreateStudentRequest
{
    [Required, StringLength(100)]
    public string? FullName { get; set; }

    [Required, EmailAddress, StringLength(100)]
    public string? Email { get; set; }

    [Required]
    public DateTimeOffset? DateOfBirth { get; set; }
}

public class UpdateStudentRequest
{
    [StringLength(100)]
    public string? FullName { get; set; }

    [EmailAddress, StringLength(100)]
    public string? Email { get; set; }

    public DateTimeOffset? DateOfBirth { get; set; }
}
