using System;
using System.Collections.Generic;

namespace PRN232.LMS.Repositories.Models;

public partial class Subject
{
    public int? Subjectid { get; set; }

    public string? Subjectcode { get; set; }

    public string? Subjectname { get; set; }

    public int? Credit { get; set; }
}
