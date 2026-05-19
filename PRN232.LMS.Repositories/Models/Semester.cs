using System;
using System.Collections.Generic;

namespace PRN232.LMS.Repositories.Models;

public partial class Semester
{
    public int? Semesterid { get; set; }

    public string? Semestername { get; set; }

    public DateTime? Startdate { get; set; }

    public DateTime? Enddate { get; set; }
}
