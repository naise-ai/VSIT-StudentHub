using System.ComponentModel.DataAnnotations;
namespace StudentManagementSystem.Models;
public class Student
{
    public int Id { get; set; }
    [Required, StringLength(20)] public string StudentId { get; set; } = "";
    [Required, StringLength(100)] public string FullName { get; set; } = "";
    [Required, EmailAddress, StringLength(150)] public string Email { get; set; } = "";
    [Phone, StringLength(20)] public string Phone { get; set; } = "";
    [Required, StringLength(80)] public string Course { get; set; } = "B.Sc. Information Technology";
    [Range(1, 8)] public int Semester { get; set; } = 5;
    [Range(0, 10)] public decimal CGPA { get; set; }
    [DataType(DataType.Date)] public DateTime DateOfBirth { get; set; }
    [DataType(DataType.Date)] public DateTime EnrollmentDate { get; set; } = DateTime.Today;
    public bool IsActive { get; set; } = true;
}
