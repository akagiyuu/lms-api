namespace PRN232.LMS.Services.Models;

public class CourseResponse
{
    public int CourseId { get; set; }
    public string CourseName { get; set; } = "";
    public int SemesterId { get; set; }
    public SemesterResponse? Semester { get; set; }
}

public class CreateCourseRequest
{
    public string CourseName { get; set; } = "";
    public int SemesterId { get; set; }
}

public class UpdateCourseRequest
{
    public string CourseName { get; set; } = "";
    public int SemesterId { get; set; }
}