using System.Runtime.Serialization;
using PRN232.LMS.Services.Common;
using PRN232.LMS.Services.Models.Response;

namespace PRN232.LMS.API.Common;

[KnownType(typeof(PagedResult<object>))]
[KnownType(typeof(CourseResponse))]
[KnownType(typeof(StudentResponse))]
[KnownType(typeof(EnrollmentResponse))]
[KnownType(typeof(SemesterResponse))]
[KnownType(typeof(SubjectResponse))]
[KnownType(typeof(TokenResponse))]
[KnownType(typeof(Dictionary<string, string[]>))]
[KnownType(typeof(System.Dynamic.ExpandoObject))]
[KnownType(typeof(object))]
public class ApiResponse<T>
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public T? Data { get; set; }
    public object? Errors { get; set; }

    public static ApiResponse<T> Ok(T data, string message = "Request processed successfully")
        => new() { Success = true, Message = message, Data = data, Errors = null };

    public static ApiResponse<T> Fail(string message, object? errors = null)
        => new() { Success = false, Message = message, Data = default, Errors = errors };
}