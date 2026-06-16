using System.ComponentModel.DataAnnotations;

namespace PRN232.LMS.Services.Models.Request;

public class CreateEnrollmentRequest
{
    [Required, Range(1, int.MaxValue)]
    public int? StudentId { get; set; }

    [Required, Range(1, int.MaxValue)]
    public int? CourseId { get; set; }

    [Required]
    public DateTimeOffset? EnrollDate { get; set; }

    [Required, RegularExpression("^(Active|Inactive|Completed)$",
        ErrorMessage = "Status must be Active, Inactive, or Completed.")]
    public string? Status { get; set; }
}

public class UpdateEnrollmentRequest
{
    [Range(1, int.MaxValue)]
    public int? StudentId { get; set; }

    [Range(1, int.MaxValue)]
    public int? CourseId { get; set; }

    public DateTimeOffset? EnrollDate { get; set; }

    [RegularExpression("^(Active|Inactive|Completed)$",
        ErrorMessage = "Status must be Active, Inactive, or Completed.")]
    public string? Status { get; set; }
}
