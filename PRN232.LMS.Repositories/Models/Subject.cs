using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PRN232.LMS.Repositories.Models;

[Table("Subject")]
public class Subject
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int SubjectId { get; set; }

    [Required]
    [MaxLength(20)]
    public string? SubjectCode { get; set; }

    [Required]
    [MaxLength(100)]
    public string? SubjectName { get; set; }

    [Required]
    public int? Credit { get; set; }
}