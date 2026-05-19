using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PRN232.LMS.Repositories.Models;

[Table("Semester")]
public class Semester
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int SemesterId { get; set; }

    [Required]
    [MaxLength(100)]
    public string? SemesterName { get; set; }

    [Required]
    public DateTimeOffset StartDate { get; set; }

    [Required]
    public DateTimeOffset EndDate { get; set; }

    public ICollection<Course> Courses { get; set; } = [];
}