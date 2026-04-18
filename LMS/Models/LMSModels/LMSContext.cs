using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace LMS.Models.LMSModels
{
    public partial class LMSContext : DbContext
    {
        public LMSContext()
        {
        }

        public LMSContext(DbContextOptions<LMSContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Administrator> Administrators { get; set; } = null!;
        public virtual DbSet<Assignment> Assignments { get; set; } = null!;
        public virtual DbSet<AssignmentCategory> AssignmentCategories { get; set; } = null!;
        public virtual DbSet<Class> Classes { get; set; } = null!;
        public virtual DbSet<Course> Courses { get; set; } = null!;
        public virtual DbSet<Department> Departments { get; set; } = null!;
        public virtual DbSet<EnrolledIn> EnrolledIns { get; set; } = null!;
        public virtual DbSet<Professor> Professors { get; set; } = null!;
        public virtual DbSet<Sshkey> Sshkeys { get; set; } = null!;
        public virtual DbSet<Student> Students { get; set; } = null!;
        public virtual DbSet<Submitted> Submitteds { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseMySql("name=LMS:LMSConnectionString", Microsoft.EntityFrameworkCore.ServerVersion.Parse("10.11.16-mariadb"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseCollation("latin1_swedish_ci")
                .HasCharSet("latin1");

            modelBuilder.Entity<Administrator>(entity =>
            {
                entity.HasKey(e => e.UserId)
                    .HasName("PRIMARY");

                entity.ToTable("Administrator");

                entity.Property(e => e.UserId)
                    .HasMaxLength(8)
                    .HasColumnName("UserID");

                entity.Property(e => e.Dob).HasColumnName("DOB");

                entity.Property(e => e.FirstName).HasMaxLength(100);

                entity.Property(e => e.LastName).HasMaxLength(100);
            });

            modelBuilder.Entity<Assignment>(entity =>
            {
                entity.ToTable("Assignment");

                entity.HasIndex(e => e.AssignmentCategoryId, "AssignmentCategoryID");

                entity.HasIndex(e => new { e.AssignmentName, e.AssignmentCategoryId }, "AssignmentName")
                    .IsUnique();

                entity.Property(e => e.AssignmentId)
                    .HasColumnType("int(11)")
                    .HasColumnName("AssignmentID");

                entity.Property(e => e.AssignmentCategoryId)
                    .HasColumnType("int(11)")
                    .HasColumnName("AssignmentCategoryID");

                entity.Property(e => e.AssignmentName).HasMaxLength(100);

                entity.Property(e => e.Contents).HasMaxLength(8192);

                entity.Property(e => e.DueDate).HasColumnType("datetime");

                entity.Property(e => e.MaxPoints).HasColumnType("int(10) unsigned");

                entity.HasOne(d => d.AssignmentCategory)
                    .WithMany(p => p.Assignments)
                    .HasForeignKey(d => d.AssignmentCategoryId)
                    .HasConstraintName("Assignment_ibfk_1");
            });

            modelBuilder.Entity<AssignmentCategory>(entity =>
            {
                entity.HasKey(e => e.CategoryId)
                    .HasName("PRIMARY");

                entity.ToTable("AssignmentCategory");

                entity.HasIndex(e => new { e.CategoryName, e.ClassId }, "CategoryName")
                    .IsUnique();

                entity.HasIndex(e => e.ClassId, "ClassID");

                entity.Property(e => e.CategoryId)
                    .HasColumnType("int(11)")
                    .HasColumnName("CategoryID");

                entity.Property(e => e.CategoryName).HasMaxLength(100);

                entity.Property(e => e.ClassId)
                    .HasColumnType("int(11)")
                    .HasColumnName("ClassID");

                entity.Property(e => e.GradeWeight).HasColumnType("smallint(5) unsigned");

                entity.HasOne(d => d.Class)
                    .WithMany(p => p.AssignmentCategories)
                    .HasForeignKey(d => d.ClassId)
                    .HasConstraintName("AssignmentCategory_ibfk_1");
            });

            modelBuilder.Entity<Class>(entity =>
            {
                entity.ToTable("Class");

                entity.HasIndex(e => new { e.CourseNum, e.CourseDeptAbbreviation }, "CourseNum");

                entity.HasIndex(e => e.ProfessorId, "ProfessorID");

                entity.HasIndex(e => new { e.Year, e.Season, e.CourseDeptAbbreviation, e.CourseNum }, "Year")
                    .IsUnique();

                entity.Property(e => e.ClassId)
                    .HasColumnType("int(11)")
                    .HasColumnName("ClassID");

                entity.Property(e => e.CourseDeptAbbreviation).HasMaxLength(4);

                entity.Property(e => e.CourseNum).HasColumnType("int(10) unsigned");

                entity.Property(e => e.EndTime).HasColumnType("time");

                entity.Property(e => e.Location).HasMaxLength(100);

                entity.Property(e => e.ProfessorId)
                    .HasMaxLength(8)
                    .HasColumnName("ProfessorID");

                entity.Property(e => e.Season).HasColumnType("enum('Spring','Fall','Summer')");

                entity.Property(e => e.StartTime).HasColumnType("time");

                entity.Property(e => e.Year).HasColumnType("smallint(5) unsigned");

                entity.HasOne(d => d.Professor)
                    .WithMany(p => p.Classes)
                    .HasForeignKey(d => d.ProfessorId)
                    .HasConstraintName("Class_ibfk_1");
            });

            modelBuilder.Entity<Course>(entity =>
            {
                entity.HasKey(e => e.CatalogId)
                    .HasName("PRIMARY");

                entity.HasIndex(e => e.CourseDeptAbbreviation, "Courses_ibfk_1");

                entity.HasIndex(e => new { e.CourseNum, e.CourseDeptAbbreviation }, "Number")
                    .IsUnique();

                entity.Property(e => e.CatalogId)
                    .HasColumnType("int(10) unsigned")
                    .HasColumnName("CatalogID");

                entity.Property(e => e.CourseDeptAbbreviation).HasMaxLength(4);

                entity.Property(e => e.CourseNum).HasColumnType("int(10) unsigned");

                entity.Property(e => e.Name).HasMaxLength(100);

                entity.HasOne(d => d.CourseDeptAbbreviationNavigation)
                    .WithMany(p => p.Courses)
                    .HasForeignKey(d => d.CourseDeptAbbreviation)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Courses_ibfk_1");
            });

            modelBuilder.Entity<Department>(entity =>
            {
                entity.HasKey(e => e.Abbreviation)
                    .HasName("PRIMARY");

                entity.ToTable("Department");

                entity.Property(e => e.Abbreviation).HasMaxLength(4);

                entity.Property(e => e.Name).HasMaxLength(100);
            });

            modelBuilder.Entity<EnrolledIn>(entity =>
            {
                entity.HasKey(e => new { e.StudentId, e.ClassId })
                    .HasName("PRIMARY")
                    .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

                entity.ToTable("EnrolledIn");

                entity.HasIndex(e => e.ClassId, "ClassID");

                entity.Property(e => e.StudentId)
                    .HasMaxLength(8)
                    .HasColumnName("StudentID");

                entity.Property(e => e.ClassId)
                    .HasColumnType("int(11)")
                    .HasColumnName("ClassID");

                entity.Property(e => e.Grade).HasMaxLength(2);

                entity.HasOne(d => d.Class)
                    .WithMany(p => p.EnrolledIns)
                    .HasForeignKey(d => d.ClassId)
                    .HasConstraintName("EnrolledIn_ibfk_2");

                entity.HasOne(d => d.Student)
                    .WithMany(p => p.EnrolledIns)
                    .HasForeignKey(d => d.StudentId)
                    .HasConstraintName("EnrolledIn_ibfk_1");
            });

            modelBuilder.Entity<Professor>(entity =>
            {
                entity.HasKey(e => e.UserId)
                    .HasName("PRIMARY");

                entity.ToTable("Professor");

                entity.HasIndex(e => e.WorkDeptAbbreviation, "WorkDeptAbbreviation");

                entity.Property(e => e.UserId)
                    .HasMaxLength(8)
                    .HasColumnName("UserID");

                entity.Property(e => e.Dob).HasColumnName("DOB");

                entity.Property(e => e.FirstName).HasMaxLength(100);

                entity.Property(e => e.LastName).HasMaxLength(100);

                entity.Property(e => e.WorkDeptAbbreviation).HasMaxLength(4);

                entity.HasOne(d => d.WorkDeptAbbreviationNavigation)
                    .WithMany(p => p.Professors)
                    .HasForeignKey(d => d.WorkDeptAbbreviation)
                    .HasConstraintName("Professor_ibfk_1");
            });

            modelBuilder.Entity<Sshkey>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("sshkey");

                entity.Property(e => e.Sshkey1)
                    .HasColumnType("text")
                    .HasColumnName("sshkey");
            });

            modelBuilder.Entity<Student>(entity =>
            {
                entity.HasKey(e => e.UserId)
                    .HasName("PRIMARY");

                entity.ToTable("Student");

                entity.HasIndex(e => e.MajorDeptAbbreviation, "MajorDeptAbbreviation");

                entity.Property(e => e.UserId)
                    .HasMaxLength(8)
                    .HasColumnName("UserID");

                entity.Property(e => e.Dob).HasColumnName("DOB");

                entity.Property(e => e.FirstName).HasMaxLength(100);

                entity.Property(e => e.LastName).HasMaxLength(100);

                entity.Property(e => e.MajorDeptAbbreviation).HasMaxLength(4);

                entity.HasOne(d => d.MajorDeptAbbreviationNavigation)
                    .WithMany(p => p.Students)
                    .HasForeignKey(d => d.MajorDeptAbbreviation)
                    .HasConstraintName("Student_ibfk_1");
            });

            modelBuilder.Entity<Submitted>(entity =>
            {
                entity.HasKey(e => new { e.StudentId, e.AssignmentId })
                    .HasName("PRIMARY")
                    .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

                entity.ToTable("Submitted");

                entity.HasIndex(e => e.AssignmentId, "AssignmentID");

                entity.Property(e => e.StudentId)
                    .HasMaxLength(8)
                    .HasColumnName("StudentID");

                entity.Property(e => e.AssignmentId)
                    .HasColumnType("int(11)")
                    .HasColumnName("AssignmentID");

                entity.Property(e => e.Contents).HasMaxLength(8192);

                entity.Property(e => e.Score).HasColumnType("int(10) unsigned");

                entity.Property(e => e.SubmissionTime).HasColumnType("datetime");

                entity.HasOne(d => d.Assignment)
                    .WithMany(p => p.Submitteds)
                    .HasForeignKey(d => d.AssignmentId)
                    .HasConstraintName("Submitted_ibfk_2");

                entity.HasOne(d => d.Student)
                    .WithMany(p => p.Submitteds)
                    .HasForeignKey(d => d.StudentId)
                    .HasConstraintName("Submitted_ibfk_1");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
