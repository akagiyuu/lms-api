namespace PRN232.LMS.Services.Models.Business;

public class StudentBusiness
{
    public int StudentId { get; set; }
    public string? FullName { get; set; }
    public string? Email { get; set; }
    public DateTimeOffset? DateOfBirth { get; set; }
    public List<EnrollmentBusiness>? Enrollments { get; set; }
}
