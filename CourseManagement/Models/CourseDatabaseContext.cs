using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace CourseManagement.Models
{
    public partial class CourseDatabaseContext : DbContext
    {
        public CourseDatabaseContext()
        {
        }

        public CourseDatabaseContext(DbContextOptions<CourseDatabaseContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Account> Accounts { get; set; }
        public virtual DbSet<Calendar> Calendars { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Class> Classes { get; set; }
        public virtual DbSet<Course> Courses { get; set; }
        public virtual DbSet<Judge> Judges { get; set; }
        public virtual DbSet<Role> Roles { get; set; }

//        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//        {
//            if (!optionsBuilder.IsConfigured)
//            {
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
//                optionsBuilder.UseSqlServer("Data Source=LAPTOP-C64GB1SV;Initial Catalog=CourseDatabase;User ID=sa;Password=Nghiakc123");
//            }
//        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>(entity =>
            {
                entity.HasKey(e => e.IdAccount);

                entity.Property(e => e.IdAccount).HasColumnName("idAccount");

                entity.Property(e => e.Active).HasColumnName("active");

                entity.Property(e => e.Fullname)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("fullname");

                entity.Property(e => e.IdRole).HasColumnName("idRole");

                entity.Property(e => e.Password)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("password");

                entity.Property(e => e.Phone)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("phone");

                entity.Property(e => e.Username)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("username");

                entity.HasOne(d => d.IdRoleNavigation)
                    .WithMany(p => p.Accounts)
                    .HasForeignKey(d => d.IdRole)
                    .HasConstraintName("FK_Accounts_Roles");
            });

            modelBuilder.Entity<Calendar>(entity =>
            {
                entity.HasKey(e => e.IdCalendar);

                entity.Property(e => e.IdCalendar).HasColumnName("idCalendar");

                entity.Property(e => e.EndTime)
                    .HasColumnType("datetime")
                    .HasColumnName("endTime");

                entity.Property(e => e.IdCourse).HasColumnName("idCourse");

                entity.Property(e => e.Length).HasColumnName("length");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("name");

                entity.Property(e => e.StartTime)
                    .HasColumnType("datetime")
                    .HasColumnName("startTime");

                entity.HasOne(d => d.IdCourseNavigation)
                    .WithMany(p => p.Calendars)
                    .HasForeignKey(d => d.IdCourse)
                    .HasConstraintName("FK_Calendars_Courses");
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(e => e.IdCategory);

                entity.Property(e => e.IdCategory).HasColumnName("idCategory");

                entity.Property(e => e.Description)
                    .IsUnicode(false)
                    .HasColumnName("description");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("name");
            });

            modelBuilder.Entity<Class>(entity =>
            {
                entity.HasKey(e => e.IdClass);

                entity.Property(e => e.IdClass).HasColumnName("idClass");

                entity.Property(e => e.IdCalendar).HasColumnName("idCalendar");

                entity.Property(e => e.IdLecturer).HasColumnName("idLecturer");

                entity.Property(e => e.IdStudent).HasColumnName("idStudent");

                entity.HasOne(d => d.IdCalendarNavigation)
                    .WithMany(p => p.Classes)
                    .HasForeignKey(d => d.IdCalendar)
                    .HasConstraintName("FK_Classes_Calendars");

                entity.HasOne(d => d.IdLecturerNavigation)
                    .WithMany(p => p.ClassIdLecturerNavigations)
                    .HasForeignKey(d => d.IdLecturer)
                    .HasConstraintName("FK_Classes_Accounts");

                entity.HasOne(d => d.IdStudentNavigation)
                    .WithMany(p => p.ClassIdStudentNavigations)
                    .HasForeignKey(d => d.IdStudent)
                    .HasConstraintName("FK_Classes_Accounts1");
            });

            modelBuilder.Entity<Course>(entity =>
            {
                entity.HasKey(e => e.IdCourse);

                entity.Property(e => e.IdCourse).HasColumnName("idCourse");

                entity.Property(e => e.Description)
                    .IsUnicode(false)
                    .HasColumnName("description");

                entity.Property(e => e.IdCategory).HasColumnName("idCategory");

                entity.Property(e => e.Image)
                    .IsUnicode(false)
                    .HasColumnName("image");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("name");

                entity.Property(e => e.Price).HasColumnName("price");

                entity.HasOne(d => d.IdCategoryNavigation)
                    .WithMany(p => p.Courses)
                    .HasForeignKey(d => d.IdCategory)
                    .HasConstraintName("FK_Courses_Categories");
            });

            modelBuilder.Entity<Judge>(entity =>
            {
                entity.HasKey(e => e.IdJudge);

                entity.Property(e => e.IdJudge).HasColumnName("idJudge");

                entity.Property(e => e.IdClass).HasColumnName("idClass");

                entity.Property(e => e.Judge1)
                    .IsUnicode(false)
                    .HasColumnName("judge");

                entity.HasOne(d => d.IdClassNavigation)
                    .WithMany(p => p.Judges)
                    .HasForeignKey(d => d.IdClass)
                    .HasConstraintName("FK_Judges_Classes");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasKey(e => e.IdRole);

                entity.Property(e => e.IdRole).HasColumnName("idRole");

                entity.Property(e => e.Description)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("description");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("name");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
