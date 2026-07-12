namespace Course.Service.Models.Response;
public class SemesterResponse
{
    public int           SemesterId   { get; set; }
    public string        SemesterName { get; set; } = null!;
    public DateTimeOffset StartDate   { get; set; }
    public DateTimeOffset EndDate     { get; set; }
}
public class SubjectResponse
{
    public int    SubjectId   { get; set; }
    public string SubjectCode { get; set; } = null!;
    public string SubjectName { get; set; } = null!;
    public int    Credit      { get; set; }
}
public class CourseResponse
{
    public int     CourseId   { get; set; }
    public string  CourseName { get; set; } = null!;
    public int     SemesterId { get; set; }
    public SemesterResponse? Semester { get; set; }
}
public class EnrollmentResponse
{
    public int            EnrollmentId { get; set; }
    public int            StudentId    { get; set; }
    public int            CourseId     { get; set; }
    public DateTimeOffset EnrollDate   { get; set; }
    public string         Status       { get; set; } = null!;
}
