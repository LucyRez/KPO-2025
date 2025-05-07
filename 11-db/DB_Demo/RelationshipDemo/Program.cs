using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

// Define the database context
public class SchoolContext : DbContext
{
    public DbSet<Student> Students { get; set; }
    public DbSet<Profile> Profiles { get; set; }
    public DbSet<Course> Courses { get; set; }
    public DbSet<StudentCourse> StudentCourses { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {     
        modelBuilder.Entity<StudentCourse>()
           .HasKey(sc => new { sc.StudentId, sc.CourseId });
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseSqlite("Data Source=school.db");
    }
}

// Define entities
public class Student
{
    [Key]
    public int Id { get; set; }

    public string Name {get; set;}

    [InverseProperty("Student")]
    public Profile? Profile { get; set; }

    public List<StudentCourse> StudentCourses { get; set; } = new();
}

public class Profile
{
    public int Id { get; set; }
    public string Email { get; set; } = "";

    [Required]
    [ForeignKey("Student")]
    public int StudentId { get; set; }

    public Student Student { get; set; } = null!;
}

public class Course
{
    [Key]
    public int Id { get; set; }
    public string Title { get; set; } = "";

    public List<StudentCourse> StudentCourses { get; set; } = new();
}

public class StudentCourse
{
    public int StudentId { get; set; }
    public Student Student { get; set; } = null!;
    public int CourseId { get; set; }
    public Course Course { get; set; } = null!;
}

class Program
{
    static void Main()
    {
        using var context = new SchoolContext();
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();

    
        try {

        var student = new Student { Name = "Alice" };
        var profile = new Profile { Email = "alice@example.com", Student = student };
        var course1 = new Course { Title = "Math" };
        var course2 = new Course { Title = "Science" };

        student.Profile = profile;
        student.StudentCourses.Add(new StudentCourse { Student = student, Course = course1 });
        student.StudentCourses.Add(new StudentCourse { Student = student, Course = course2 });

        var student2 = new Student { Name = "Bob" };


        context.Students.Add(student);
        context.SaveChanges();


        context.Students.Add(student2);
        context.SaveChanges();

       
        } catch {
        }

        Console.WriteLine("Database created and data seeded.");
    }
}
