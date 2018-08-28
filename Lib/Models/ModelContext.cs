namespace Lib.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class ModelContext : DbContext
    {
        public ModelContext()
            : base("name=ModelContext")
        {
        }

        public virtual DbSet<Admin> Admins { get; set; }
        public virtual DbSet<Book> Books { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Major> Majors { get; set; }
        public virtual DbSet<Student> Students { get; set; }
        public virtual DbSet<StudentBook> StudentBooks { get; set; }
        public virtual DbSet<vBookDetail> vBookDetails { get; set; }
        public virtual DbSet<vStudentDetail> vStudentDetails { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Admin>()
                .Property(e => e.AdminName)
                .IsUnicode(false);

            modelBuilder.Entity<Admin>()
                .Property(e => e.Password)
                .IsUnicode(false);

            modelBuilder.Entity<Book>()
                .Property(e => e.ISBN)
                .IsUnicode(false);

            modelBuilder.Entity<Book>()
                .Property(e => e.Title)
                .IsUnicode(false);

            modelBuilder.Entity<Book>()
                .Property(e => e.Author)
                .IsUnicode(false);

            modelBuilder.Entity<Book>()
                .Property(e => e.Image)
                .IsUnicode(false);

            modelBuilder.Entity<Book>()
                .HasMany(e => e.StudentBooks)
                .WithRequired(e => e.Book)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Category>()
                .Property(e => e.CategoryName)
                .IsUnicode(false);

            modelBuilder.Entity<Major>()
                .Property(e => e.MajorName)
                .IsUnicode(false);

            modelBuilder.Entity<Major>()
                .Property(e => e.Phone)
                .IsUnicode(false);

            modelBuilder.Entity<Student>()
                .Property(e => e.StudentName)
                .IsUnicode(false);

            modelBuilder.Entity<Student>()
                .Property(e => e.Password)
                .IsUnicode(false);

            modelBuilder.Entity<Student>()
                .Property(e => e.Address)
                .IsUnicode(false);

            modelBuilder.Entity<Student>()
                .Property(e => e.Phone)
                .IsUnicode(false);

            modelBuilder.Entity<Student>()
                .HasMany(e => e.StudentBooks)
                .WithRequired(e => e.Student)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<StudentBook>()
                .Property(e => e.IssueStatus)
                .IsUnicode(false);

            modelBuilder.Entity<vBookDetail>()
                .Property(e => e.ISBN)
                .IsUnicode(false);

            modelBuilder.Entity<vBookDetail>()
                .Property(e => e.Title)
                .IsUnicode(false);

            modelBuilder.Entity<vBookDetail>()
                .Property(e => e.Author)
                .IsUnicode(false);

            modelBuilder.Entity<vBookDetail>()
                .Property(e => e.StudentName)
                .IsUnicode(false);

            modelBuilder.Entity<vBookDetail>()
                .Property(e => e.IssueStatus)
                .IsUnicode(false);

            modelBuilder.Entity<vStudentDetail>()
                .Property(e => e.Title)
                .IsUnicode(false);

            modelBuilder.Entity<vStudentDetail>()
                .Property(e => e.StudentName)
                .IsUnicode(false);

            modelBuilder.Entity<vStudentDetail>()
                .Property(e => e.Address)
                .IsUnicode(false);

            modelBuilder.Entity<vStudentDetail>()
                .Property(e => e.Phone)
                .IsUnicode(false);
        }
    }
}
