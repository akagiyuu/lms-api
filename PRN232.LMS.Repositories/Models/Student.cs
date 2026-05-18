using System;
using System.Collections.Generic;

namespace PRN232.LMS.Repositories.Models;

public partial class Student
{
    public int? Studentid { get; set; }

    public string? Fullname { get; set; }

    public string? Email { get; set; }

    public DateTime? Dateofbirth { get; set; }
}
