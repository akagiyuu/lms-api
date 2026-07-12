using Course.Service.Models;
using Microsoft.EntityFrameworkCore;
namespace Course.Service.Services;
public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<SemesterEntity>   Semesters   => Set<SemesterEntity>();
    public DbSet<SubjectEntity>    Subjects    => Set<SubjectEntity>();
    public DbSet<CourseEntity>     Courses     => Set<CourseEntity>();
    public DbSet<EnrollmentEntity> Enrollments => Set<EnrollmentEntity>();
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<SemesterEntity>(e =>
        {
            e.ToTable("Semester");
            e.HasKey(s => s.SemesterId);
            e.Property(s => s.SemesterId).HasColumnName("SemesterId").ValueGeneratedOnAdd();
            e.Property(s => s.SemesterName).HasColumnName("SemesterName").HasMaxLength(100).IsRequired();
            e.Property(s => s.StartDate).HasColumnName("StartDate").IsRequired();
            e.Property(s => s.EndDate).HasColumnName("EndDate").IsRequired();
        });
        modelBuilder.Entity<SubjectEntity>(e =>
        {
            e.ToTable("Subject");
            e.HasKey(s => s.SubjectId);
            e.Property(s => s.SubjectId).HasColumnName("SubjectId").ValueGeneratedOnAdd();
            e.Property(s => s.SubjectCode).HasColumnName("SubjectCode").HasMaxLength(20).IsRequired();
            e.Property(s => s.SubjectName).HasColumnName("SubjectName").HasMaxLength(100).IsRequired();
            e.Property(s => s.Credit).HasColumnName("Credit").IsRequired();
        });
        modelBuilder.Entity<CourseEntity>(e =>
        {
            e.ToTable("Course");
            e.HasKey(c => c.CourseId);
            e.Property(c => c.CourseId).HasColumnName("CourseId").ValueGeneratedOnAdd();
            e.Property(c => c.CourseName).HasColumnName("CourseName").HasMaxLength(100).IsRequired();
            e.Property(c => c.SemesterId).HasColumnName("SemesterId").IsRequired();
            e.HasOne(c => c.Semester).WithMany(s => s.Courses).HasForeignKey(c => c.SemesterId);
        });
        modelBuilder.Entity<EnrollmentEntity>(e =>
        {
            e.ToTable("Enrollment");
            e.HasKey(en => en.EnrollmentId);
            e.Property(en => en.EnrollmentId).HasColumnName("EnrollmentId").ValueGeneratedOnAdd();
            e.Property(en => en.StudentId).HasColumnName("StudentId").IsRequired();
            e.Property(en => en.CourseId).HasColumnName("CourseId").IsRequired();
            e.Property(en => en.EnrollDate).HasColumnName("EnrollDate").IsRequired();
            e.Property(en => en.Status).HasColumnName("Status").HasMaxLength(20).IsRequired();
            e.HasOne(en => en.Course).WithMany(c => c.Enrollments).HasForeignKey(en => en.CourseId);
        });
    }
}
