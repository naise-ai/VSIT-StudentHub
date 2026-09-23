using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Data;
namespace StudentManagementSystem.Controllers;
[Authorize] public class DashboardController(AppDbContext db) : Controller
{
    public async Task<IActionResult> Index()
    {
        var students = await db.Students.AsNoTracking().ToListAsync();
        ViewBag.Total = students.Count; ViewBag.Active = students.Count(x => x.IsActive); ViewBag.Inactive = students.Count(x => !x.IsActive); ViewBag.Average = students.Count == 0 ? 0 : Math.Round(students.Average(x => (double)x.CGPA), 2); ViewBag.Courses = students.Select(x => x.Course).Distinct().Count();
        ViewBag.TopStudents = students.OrderByDescending(x => x.CGPA).Take(5).ToList();
        ViewBag.CourseStats = students.GroupBy(x => x.Course).Select(g => new { Name = g.Key, Count = g.Count() }).OrderByDescending(x => x.Count).ToList();
        ViewBag.Recent = students.OrderByDescending(x => x.EnrollmentDate).Take(6).ToList();
        return View();
    }
    public IActionResult Error() => View();
}
