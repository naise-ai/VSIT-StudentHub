using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Models;
namespace StudentManagementSystem.Data;
public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options) { public DbSet<Student> Students => Set<Student>(); }
