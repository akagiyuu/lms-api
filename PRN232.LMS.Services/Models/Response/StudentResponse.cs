namespace PRN232.LMS.Services.Models.Response;

public class BasicStudentResponse
{
    public int StudentId { get; set; }
    public string? FullName { get; set; }
    public string? Email { get; set; }
}

public class StudentResponse : BasicStudentResponse
{
    public DateTimeOffset DateOfBirth { get; set; }
    public List<BasicEnrollmentResponse>? Enrollments { get; set; }
}
