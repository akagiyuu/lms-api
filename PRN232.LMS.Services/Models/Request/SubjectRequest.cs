using System.ComponentModel.DataAnnotations;

namespace PRN232.LMS.Services.Models.Request;

public class CreateSubjectRequest
{
    [Required, StringLength(20)]
    public string? SubjectCode { get; set; }

    [Required, StringLength(100)]
    public string? SubjectName { get; set; }

    [Required, Range(1, 10, ErrorMessage = "Credit must be between 1 and 10.")]
    public int? Credit { get; set; }
}

public class UpdateSubjectRequest
{
    [StringLength(20)]
    public string? SubjectCode { get; set; }

    [StringLength(100)]
    public string? SubjectName { get; set; }

    [Range(1, 10, ErrorMessage = "Credit must be between 1 and 10.")]
    public int? Credit { get; set; }
}
