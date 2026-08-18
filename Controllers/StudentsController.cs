using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Data;
using StudentManagementSystem.Models;
namespace StudentManagementSystem.Controllers;
[Authorize] public class StudentsController(AppDbContext db) : Controller
{
    public async Task<IActionResult> Index(string? search, string? course, int? semester, bool? active)
    {
        var q = db.Students.AsNoTracking().AsQueryable();
        if (!string.IsNullOrWhiteSpace(search)) q = q.Where(s => s.FullName.Contains(search) || s.StudentId.Contains(search) || s.Email.Contains(search));
        if (!string.IsNullOrWhiteSpace(course)) q = q.Where(s => s.Course == course);
        if (semester.HasValue) q = q.Where(s => s.Semester == semester.Value);
        if (active.HasValue) q = q.Where(s => s.IsActive == active.Value);
        ViewBag.Search = search; ViewBag.Course = course; ViewBag.Semester = semester; ViewBag.Active = active; ViewBag.Courses = await db.Students.Select(s => s.Course).Distinct().OrderBy(x => x).ToListAsync();
        return View(await q.OrderBy(s => s.FullName).ToListAsync());
    }
    public async Task<IActionResult> Details(int id) { var s = await db.Students.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id); return s == null ? NotFound() : View(s); }
    [HttpGet] public IActionResult Create() => View(new Student { DateOfBirth = DateTime.Today.AddYears(-18), EnrollmentDate = DateTime.Today });
    [HttpPost, ValidateAntiForgeryToken] public async Task<IActionResult> Create(Student student) { if (await db.Students.AnyAsync(s => s.StudentId == student.StudentId)) ModelState.AddModelError("StudentId", "Student ID already exists."); if (!ModelState.IsValid) return View(student); db.Add(student); await db.SaveChangesAsync(); TempData["Success"] = "Student added successfully."; return RedirectToAction(nameof(Index)); }
    [HttpGet] public async Task<IActionResult> Edit(int id) { var s = await db.Students.FindAsync(id); return s == null ? NotFound() : View(s); }
    [HttpPost, ValidateAntiForgeryToken] public async Task<IActionResult> Edit(int id, Student student) { if (id != student.Id) return NotFound(); if (await db.Students.AnyAsync(s => s.StudentId == student.StudentId && s.Id != id)) ModelState.AddModelError("StudentId", "Student ID already exists."); if (!ModelState.IsValid) return View(student); db.Update(student); await db.SaveChangesAsync(); TempData["Success"] = "Student updated successfully."; return RedirectToAction(nameof(Index)); }
    [HttpPost, ValidateAntiForgeryToken] public async Task<IActionResult> Delete(int id) { var s = await db.Students.FindAsync(id); if (s != null) { db.Students.Remove(s); await db.SaveChangesAsync(); TempData["Success"] = "Student deleted."; } return RedirectToAction(nameof(Index)); }
    [HttpPost, ValidateAntiForgeryToken] public async Task<IActionResult> ToggleStatus(int id) { var s = await db.Students.FindAsync(id); if (s == null) return NotFound(); s.IsActive = !s.IsActive; await db.SaveChangesAsync(); TempData["Success"] = $"Student marked {(s.IsActive ? "active" : "inactive")}."; return RedirectToAction(nameof(Index)); }
    public async Task<FileResult> ExportCsv() { var all = await db.Students.AsNoTracking().OrderBy(x => x.StudentId).ToListAsync(); var sb = new StringBuilder("Student ID,Full Name,Email,Phone,Course,Semester,CGPA,Status\n"); foreach (var s in all) sb.AppendLine(string.Join(',', Q(s.StudentId), Q(s.FullName), Q(s.Email), Q(s.Phone), Q(s.Course), s.Semester, s.CGPA, Q(s.IsActive ? "Active" : "Inactive"))); return File(Encoding.UTF8.GetBytes(sb.ToString()), "text/csv", $"students-{DateTime.Now:yyyyMMdd}.csv"); }
    static string Q(string? x) => $"\"{(x ?? "").Replace("\"", "\"\"")}\"";
}
