namespace PRN232.LMS.Services.Models.Response;

public class BasicSemesterResponse
{
    public int SemesterId { get; set; }
    public string? SemesterName { get; set; }
    public DateTimeOffset? StartDate { get; set; }
    public DateTimeOffset? EndDate { get; set; }
}

public class SemesterResponse : BasicSemesterResponse
{
    public List<BasicCourseResponse>? Courses { get; set; }
}
