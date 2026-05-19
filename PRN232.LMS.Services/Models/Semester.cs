using System.Dynamic;

namespace PRN232.LMS.Services.Models;

public class SemesterResponse
{
    public int SemesterId { get; set; }
    public string SemesterName { get; set; } = "";
    public DateTime EndDate { get; set; }
}

public class CreateSemesterRequest
{
    public string SemesterName { get; set; } = "";
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}

public class UpdateSemesterRequest
{
    public string SemesterName { get; set; } = "";
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}