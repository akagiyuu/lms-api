namespace Course.Service.Models;
public class SemesterEntity
{
    public int      SemesterId   { get; set; }
    public string   SemesterName { get; set; } = null!;
    public DateTime StartDate    { get; set; }
    public DateTime EndDate      { get; set; }
    public virtual ICollection<CourseEntity> Courses { get; set; } = new List<CourseEntity>();
}
public class SubjectEntity
{
    public int    SubjectId   { get; set; }
    public string SubjectCode { get; set; } = null!;
    public string SubjectName { get; set; } = null!;
    public int    Credit      { get; set; }
}
public class CourseEntity
{
    public int    CourseId   { get; set; }
    public string CourseName { get; set; } = null!;
    public int    SemesterId { get; set; }
    public virtual SemesterEntity  Semester    { get; set; } = null!;
    public virtual ICollection<EnrollmentEntity> Enrollments { get; set; } = new List<EnrollmentEntity>();
}
public class EnrollmentEntity
{
    public int      EnrollmentId { get; set; }
    public int      StudentId    { get; set; }
    public int      CourseId     { get; set; }
    public DateTime EnrollDate   { get; set; }
    public string   Status       { get; set; } = null!;
    public virtual CourseEntity Course { get; set; } = null!;
}
