namespace PRN232.LMS.Services.Models;

public class StudentResponse
{
    public int StudentId { get; set; }
    public string FullName { get; set; } = "";
    public string Email { get; set; } = "";
    public DateTime DateOfBirth { get; set; }
}

public class CreateStudentRequest
{
    public string FullName { get; set; } = "";
    public string Email { get; set; } = "";
    public DateTime DateOfBirth { get; set; }
}

public class UpdateStudentRequest
{
    public string FullName { get; set; } = "";
    public string Email { get; set; } = "";
    public DateTime DateOfBirth { get; set; }
}