using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace PRN232.LMS.Repositories.Models;

public partial class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Course> Courses { get; set; }

    public virtual DbSet<Enrollment> Enrollments { get; set; }

    public virtual DbSet<Semester> Semesters { get; set; }

    public virtual DbSet<Student> Students { get; set; }

    public virtual DbSet<Subject> Subjects { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Course>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("course");

            entity.Property(e => e.Courseid).HasColumnName("courseid");
            entity.Property(e => e.Coursename)
                .HasMaxLength(100)
                .HasColumnName("coursename");
            entity.Property(e => e.Semesterid).HasColumnName("semesterid");
        });

        modelBuilder.Entity<Enrollment>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("enrollment");

            entity.Property(e => e.Courseid).HasColumnName("courseid");
            entity.Property(e => e.Enrolldate).HasColumnName("enrolldate");
            entity.Property(e => e.Enrollmentid).HasColumnName("enrollmentid");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasColumnName("status");
            entity.Property(e => e.Studentid).HasColumnName("studentid");
        });

        modelBuilder.Entity<Semester>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("semester");

            entity.Property(e => e.Enddate).HasColumnName("enddate");
            entity.Property(e => e.Semesterid).HasColumnName("semesterid");
            entity.Property(e => e.Semestername)
                .HasMaxLength(100)
                .HasColumnName("semestername");
            entity.Property(e => e.Startdate).HasColumnName("startdate");
        });

        modelBuilder.Entity<Student>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("student");

            entity.Property(e => e.Dateofbirth).HasColumnName("dateofbirth");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("email");
            entity.Property(e => e.Fullname)
                .HasMaxLength(100)
                .HasColumnName("fullname");
            entity.Property(e => e.Studentid).HasColumnName("studentid");
        });

        modelBuilder.Entity<Subject>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("subject");

            entity.Property(e => e.Credit).HasColumnName("credit");
            entity.Property(e => e.Subjectcode)
                .HasMaxLength(20)
                .HasColumnName("subjectcode");
            entity.Property(e => e.Subjectid).HasColumnName("subjectid");
            entity.Property(e => e.Subjectname)
                .HasMaxLength(100)
                .HasColumnName("subjectname");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
