using Microsoft.EntityFrameworkCore;
using Student.Service.Models;
namespace Student.Service.Services;
public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<StudentEntity> Students => Set<StudentEntity>();
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<StudentEntity>(e =>
        {
            e.ToTable("Student");
            e.HasKey(s => s.StudentId);
            e.Property(s => s.StudentId).HasColumnName("StudentId").ValueGeneratedOnAdd();
            e.Property(s => s.FullName).HasColumnName("FullName").HasMaxLength(100).IsRequired();
            e.Property(s => s.Email).HasColumnName("Email").HasMaxLength(100).IsRequired();
            e.Property(s => s.DateOfBirth).HasColumnName("DateOfBirth").IsRequired();
        });
    }
}
