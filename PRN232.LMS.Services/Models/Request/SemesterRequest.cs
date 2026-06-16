using System.ComponentModel.DataAnnotations;

namespace PRN232.LMS.Services.Models.Request;

public class CreateSemesterRequest
{
    [Required, StringLength(100)]
    public string? SemesterName { get; set; }

    [Required]
    public DateTimeOffset? StartDate { get; set; }

    [Required]
    public DateTimeOffset? EndDate { get; set; }
}

public class UpdateSemesterRequest
{
    [StringLength(100)]
    public string? SemesterName { get; set; }

    public DateTimeOffset? StartDate { get; set; }

    public DateTimeOffset? EndDate { get; set; }
}
