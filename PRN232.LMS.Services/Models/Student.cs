namespace PRN232.LMS.Services.Models;

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

public class CreateStudentRequest
{
    public string? FullName { get; set; }
    public string? Email { get; set; }
    public DateTimeOffset? DateOfBirth { get; set; }
}

public class UpdateStudentRequest
{
    public string? FullName { get; set; }
    public string? Email { get; set; }
    public DateTimeOffset? DateOfBirth { get; set; }
}