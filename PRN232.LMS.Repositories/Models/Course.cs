using System;
using System.Collections.Generic;

namespace PRN232.LMS.Repositories.Models;

public partial class Course
{
    public int? Courseid { get; set; }

    public string? Coursename { get; set; }

    public int? Semesterid { get; set; }
}
