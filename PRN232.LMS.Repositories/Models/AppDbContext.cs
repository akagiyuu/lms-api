using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace PRN232.LMS.Repositories.Models;

public partial class AppDbContext : DbContext
{
    public AppDbContext() { }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public virtual DbSet<Course> Courses { get; set; }
    public virtual DbSet<Enrollment> Enrollments { get; set; }
    public virtual DbSet<Semester> Semesters { get; set; }
    public virtual DbSet<Student> Students { get; set; }
    public virtual DbSet<Subject> Subjects { get; set; }
    public virtual DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Course>(entity =>
        {
            entity.HasKey(e => e.CourseId).HasName("Course_pkey");
            entity.ToTable("Course");
            entity.HasIndex(e => e.SemesterId, "IX_Course_SemesterId");
            entity.Property(e => e.CourseName).HasMaxLength(100);
            entity.HasOne(d => d.Semester).WithMany(p => p.Courses)
                .HasForeignKey(d => d.SemesterId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("Course_SemesterId_fkey");
        });

        modelBuilder.Entity<Enrollment>(entity =>
        {
            entity.HasKey(e => e.EnrollmentId).HasName("Enrollment_pkey");
            entity.ToTable("Enrollment");
            entity.HasIndex(e => e.CourseId, "IX_Enrollment_CourseId");
            entity.HasIndex(e => e.StudentId, "IX_Enrollment_StudentId");
            entity.Property(e => e.Status).HasMaxLength(20);
            entity.HasOne(d => d.Course).WithMany(p => p.Enrollments)
                .HasForeignKey(d => d.CourseId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("Enrollment_CourseId_fkey");
            entity.HasOne(d => d.Student).WithMany(p => p.Enrollments)
                .HasForeignKey(d => d.StudentId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("Enrollment_StudentId_fkey");
        });

        modelBuilder.Entity<Semester>(entity =>
        {
            entity.HasKey(e => e.SemesterId).HasName("Semester_pkey");
            entity.ToTable("Semester");
            entity.Property(e => e.SemesterName).HasMaxLength(100);
        });

        modelBuilder.Entity<Student>(entity =>
        {
            entity.HasKey(e => e.StudentId).HasName("Student_pkey");
            entity.ToTable("Student");
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.FullName).HasMaxLength(100);
        });

        modelBuilder.Entity<Subject>(entity =>
        {
            entity.HasKey(e => e.SubjectId).HasName("Subject_pkey");
            entity.ToTable("Subject");
            entity.Property(e => e.SubjectCode).HasMaxLength(20);
            entity.Property(e => e.SubjectName).HasMaxLength(100);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("User_pkey");
            entity.ToTable("User");
            entity.HasIndex(e => e.Username).IsUnique();
            entity.Property(e => e.Username).HasMaxLength(50);
            entity.Property(e => e.PasswordHash).HasMaxLength(255);
            entity.Property(e => e.Role).HasMaxLength(20);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
