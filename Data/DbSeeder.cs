using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Models;
namespace StudentManagementSystem.Data;
public static class DbSeeder
{
    public static async Task SeedAsync(AppDbContext db)
    {
        if (await db.Students.AnyAsync()) return;
        var names = new[] { "Aarav Sharma", "Ananya Patel", "Rohan Mehta", "Isha Shah", "Kabir Joshi", "Sneha Kulkarni", "Aditya Singh", "Mira Desai", "Arjun Nair", "Priya Verma", "Rahul Gupta", "Neha Kapoor" };
        var courses = new[] { "B.Sc. Information Technology", "BCA", "B.Sc. Computer Science", "B.Com. IT" };
        for (int i = 0; i < names.Length; i++) db.Students.Add(new Student { StudentId = $"VSIT{2026001 + i}", FullName = names[i], Email = $"{names[i].ToLower().Replace(" ", ".")}@vsit.edu", Phone = $"98{10000000 + i * 731}", Course = courses[i % courses.Length], Semester = 3 + i % 4, CGPA = Math.Round(6.8m + (i * 0.23m) % 2.6m, 2), DateOfBirth = new DateTime(2004 + i % 3, 2 + i % 10, 5 + i % 20), EnrollmentDate = new DateTime(2024, 7, 1).AddDays(i * 11), IsActive = i != 8 });
        await db.SaveChangesAsync();
    }
}
