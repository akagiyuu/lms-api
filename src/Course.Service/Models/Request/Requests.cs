using System.ComponentModel.DataAnnotations;
namespace Course.Service.Models.Request;
public class CreateSemesterRequest
{
    [Required, MaxLength(100)] public string SemesterName { get; set; } = null!;
    [Required] public DateTimeOffset? StartDate { get; set; }
    [Required] public DateTimeOffset? EndDate   { get; set; }
}
public class UpdateSemesterRequest
{
    [MaxLength(100)] public string? SemesterName { get; set; }
    public DateTimeOffset? StartDate { get; set; }
    public DateTimeOffset? EndDate   { get; set; }
}
public class CreateSubjectRequest
{
    [Required, MaxLength(20)]  public string SubjectCode { get; set; } = null!;
    [Required, MaxLength(100)] public string SubjectName { get; set; } = null!;
    [Required, Range(1, 10)]   public int?   Credit      { get; set; }
}
public class UpdateSubjectRequest
{
    [MaxLength(20)]  public string? SubjectCode { get; set; }
    [MaxLength(100)] public string? SubjectName { get; set; }
    [Range(1, 10)]   public int?    Credit      { get; set; }
}
public class CreateCourseRequest
{
    [Required, MaxLength(100)] public string CourseName { get; set; } = null!;
    [Required, Range(1, int.MaxValue)] public int? SemesterId { get; set; }
}
public class UpdateCourseRequest
{
    [MaxLength(100)] public string? CourseName { get; set; }
    [Range(1, int.MaxValue)] public int? SemesterId { get; set; }
}
public class CreateEnrollmentRequest
{
    [Required, Range(1, int.MaxValue)] public int? StudentId  { get; set; }
    [Required, Range(1, int.MaxValue)] public int? CourseId   { get; set; }
    [Required] public DateTimeOffset? EnrollDate { get; set; }
    [Required, RegularExpression("^(Active|Inactive|Completed)$")] public string? Status { get; set; }
}
public class UpdateEnrollmentRequest
{
    [Range(1, int.MaxValue)] public int? StudentId  { get; set; }
    [Range(1, int.MaxValue)] public int? CourseId   { get; set; }
    public DateTimeOffset? EnrollDate { get; set; }
    [RegularExpression("^(Active|Inactive|Completed)$")] public string? Status { get; set; }
}
