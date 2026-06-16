namespace PRN232.LMS.Services.Models.Business;

public class SemesterBusiness
{
    public int SemesterId { get; set; }
    public string? SemesterName { get; set; }
    public DateTimeOffset? StartDate { get; set; }
    public DateTimeOffset? EndDate { get; set; }
    public List<CourseBusiness>? Courses { get; set; }
}
