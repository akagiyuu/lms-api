namespace PRN232.LMS.Services.Models.Response;

public class BasicCourseResponse
{
    public int CourseId { get; set; }
    public string? CourseName { get; set; }
    public int? SemesterId { get; set; }
}

public class CourseResponse : BasicCourseResponse
{
    public BasicSemesterResponse? Semester { get; set; }
    public List<BasicEnrollmentResponse>? Enrollments { get; set; }
}
