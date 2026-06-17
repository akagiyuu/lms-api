using System.Runtime.Serialization;
using PRN232.LMS.Services.Models.Response;

namespace PRN232.LMS.Services.Common;

[KnownType(typeof(CourseResponse))]
[KnownType(typeof(StudentResponse))]
[KnownType(typeof(EnrollmentResponse))]
[KnownType(typeof(SemesterResponse))]
[KnownType(typeof(SubjectResponse))]
[KnownType(typeof(TokenResponse))]
[KnownType(typeof(object))]
[KnownType(typeof(System.Dynamic.ExpandoObject))]
public class PagedResult<T>
{
    public int Page { get; set; }
    public int Size { get; set; }
    public int TotalItems { get; set; }
    public int TotalPages { get; set; }
    public List<T> Items { get; set; } = [];
}
