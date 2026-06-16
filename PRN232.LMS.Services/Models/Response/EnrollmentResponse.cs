namespace PRN232.LMS.Services.Models.Response;

public class BasicEnrollmentResponse
{
    public int EnrollmentId { get; set; }
    public DateTimeOffset? EnrollDate { get; set; }
    public string? Status { get; set; }
}

public class EnrollmentResponse : BasicEnrollmentResponse
{
    public int? StudentId { get; set; }
    public int? CourseId { get; set; }
    public BasicStudentResponse? Student { get; set; }
    public BasicCourseResponse? Course { get; set; }
}
