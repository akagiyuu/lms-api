using System;
using System.Collections.Generic;

namespace PRN232.LMS.Repositories.Models;

public partial class Enrollment
{
    public int? Enrollmentid { get; set; }

    public int? Studentid { get; set; }

    public int? Courseid { get; set; }

    public DateTime? Enrolldate { get; set; }

    public string? Status { get; set; }
}
