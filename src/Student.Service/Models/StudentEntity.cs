namespace Student.Service.Models;
public class StudentEntity
{
    public int      StudentId   { get; set; }
    public string   FullName    { get; set; } = null!;
    public string   Email       { get; set; } = null!;
    public DateTime DateOfBirth { get; set; }
}
