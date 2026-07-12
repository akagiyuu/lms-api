namespace Student.Service.Models.Response;
public class StudentResponse
{
    public int           StudentId   { get; set; }
    public string        FullName    { get; set; } = null!;
    public string        Email       { get; set; } = null!;
    public DateTimeOffset DateOfBirth { get; set; }
}
