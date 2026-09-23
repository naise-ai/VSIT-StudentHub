using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Models;

namespace StudentManagementSystem.Data;

public static class DbSeeder
{
    private static readonly string[] Courses =
    [
        "B.Sc. Information Technology",
        "BCA",
        "B.Sc. Computer Science",
        "B.Com"
    ];

    private static readonly string[] CourseCodes = ["302", "301", "303", "304"];

    private static readonly string[] FirstNames =
    [
        "Aarav", "Aisha", "Ananya", "Arjun", "Avni", "Dev", "Diya", "Eshan", "Fatima", "Gauri",
        "Harsh", "Ibrahim", "Isha", "Kabir", "Kavya", "Manav", "Meera", "Nandini", "Neel", "Nikhil",
        "Pooja", "Raghav", "Riya", "Saanvi", "Sameer", "Sana", "Sara", "Shaurya", "Simran", "Tanya",
        "Varun", "Vedika", "Vihaan", "Yash", "Zara"
    ];

    private static readonly string[] Surnames =
    [
        "Sharma", "Patel", "Mehta", "Shah", "Joshi", "Nair", "Verma", "Kapoor", "Desai", "Kulkarni",
        "Singh", "Khan", "Chatterjee", "Fernandes", "Iyer", "Malhotra", "Bose", "Menon", "Thomas", "Bhat",
        "Gupta", "Reddy", "Saxena", "Mishra", "Sethi", "D'Souza", "Banerjee", "Kaur", "Qureshi", "Pillai"
    ];

    public static async Task SeedAsync(AppDbContext db)
    {
        var existing = await db.Students.OrderBy(s => s.Id).ToListAsync();

        for (var i = 0; i < existing.Count; i++)
        {
            var courseIndex = existing[i].Course == "B.Com. IT"
                ? 3
                : Array.IndexOf(Courses, existing[i].Course);
            if (courseIndex < 0) courseIndex = i % Courses.Length;
            var enrollmentYear = existing[i].EnrollmentDate.Year is >= 2024 and <= 2026
                ? existing[i].EnrollmentDate.Year
                : 2025;
            existing[i].Course = Courses[courseIndex];
            existing[i].StudentId = CreateRollNumber(enrollmentYear, courseIndex, i);
            existing[i].Email = CreateEmail(existing[i].FullName);
        }

        var recordsToAdd = Math.Max(0, 100 - Math.Max(0, existing.Count - 12));
        for (var i = 0; i < recordsToAdd; i++)
        {
            var nameIndex = (i + 12) % (FirstNames.Length * Surnames.Length);
            var fullName = $"{FirstNames[nameIndex % FirstNames.Length]} {Surnames[nameIndex / FirstNames.Length]}";
            var courseIndex = i % Courses.Length;
            var enrollmentYear = 2024 + i % 3;
            var enrollmentDate = new DateTime(enrollmentYear, 7, 1).AddDays(i % 28);

            db.Students.Add(new Student
            {
                StudentId = CreateRollNumber(enrollmentYear, courseIndex, existing.Count + i),
                FullName = fullName,
                Email = CreateEmail(fullName),
                Phone = $"+91 {70000 + (i * 137) % 30000:D5} {10000 + (i * 7919) % 90000:D5}",
                Course = Courses[courseIndex],
                Semester = 1 + (enrollmentYear == 2024 ? 4 : enrollmentYear == 2025 ? 2 : 0) + i % 4,
                CGPA = Math.Round(6.4m + (i * 0.19m) % 3.3m, 2),
                DateOfBirth = new DateTime(2002 + i % 4, 1 + i % 12, 1 + i % 27),
                EnrollmentDate = enrollmentDate,
                IsActive = i % 11 != 0
            });
        }

        await db.SaveChangesAsync();
    }

    private static string CreateRollNumber(int year, int courseIndex, int position)
    {
        var division = (char)('A' + position / 60 % 4);
        var rollNumber = position % 60 + 1;
        return $"2{year % 10}{CourseCodes[courseIndex]}{division}{rollNumber:0000}";
    }

    private static string CreateEmail(string fullName)
    {
        var parts = fullName.ToLowerInvariant()
            .Replace("'", string.Empty)
            .Split(' ', StringSplitOptions.RemoveEmptyEntries);
        return $"{parts[0]}.{parts[^1]}@vsit.edu.in";
    }
}
