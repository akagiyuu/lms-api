namespace PRN232.LMS.Services.Models;

public class SubjectResponse
{
    public int SubjectId { get; set; }
    public string? SubjectCode { get; set; }
    public string? SubjectName { get; set; }
    public int Credit { get; set; }
}

public class CreateSubjectRequest
{
    public string? SubjectCode { get; set; }
    public string? SubjectName { get; set; }
    public int Credit { get; set; }
}

public class UpdateSubjectRequest
{
    public string? SubjectCode { get; set; }
    public string? SubjectName { get; set; }
    public int? Credit { get; set; }
}