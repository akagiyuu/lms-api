using System.ComponentModel.DataAnnotations;

namespace PRN232.LMS.Services.Models.Request;

public class CreateCourseRequest
{
    [Required, StringLength(100)]
    public string? CourseName { get; set; }

    [Required, Range(1, int.MaxValue, ErrorMessage = "SemesterId must be a positive integer.")]
    public int? SemesterId { get; set; }
}

public class UpdateCourseRequest
{
    [StringLength(100)]
    public string? CourseName { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "SemesterId must be a positive integer.")]
    public int? SemesterId { get; set; }
}
