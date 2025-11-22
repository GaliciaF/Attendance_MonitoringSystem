using AttendanceSystem.Data;
using AttendanceSystem.Models;
using Microsoft.AspNetCore.Mvc;

namespace AttendanceSystem.Controllers
{
    public class RegistrationController : Controller
    {
        private readonly AppDbContext _context;

        public RegistrationController(AppDbContext context)
        {
            _context = context;
        }

        // Show registration form
        public IActionResult Index() => View();

        [HttpPost]
        public IActionResult Index(
            string role,
            string adminUsername, string adminPassword,
            string studentName, string studentNo, string studentEmail, string studentPassword)
        {
            if (string.IsNullOrEmpty(role))
            {
                ViewBag.Message = "Please select a valid role.";
                return View();
            }

            if (role == "Admin")
            {
                if (string.IsNullOrEmpty(adminUsername) || string.IsNullOrEmpty(adminPassword))
                {
                    ViewBag.Message = "All admin fields are required!";
                    return View();
                }

                if (_context.Admins.Any(a => a.Username == adminUsername))
                {
                    ViewBag.Message = "Admin username already exists!";
                    return View();
                }

                var admin = new Admin
                {
                    Username = adminUsername,
                    Password = BCrypt.Net.BCrypt.HashPassword(adminPassword),
                    IsApproved = false
                };
                _context.Admins.Add(admin);
                _context.SaveChanges();


                ViewBag.Message = "Admin registered successfully!";
                return RedirectToAction("RegistrationPending", "Registration");
            }
            else if (role == "Student")
            {
                if (string.IsNullOrEmpty(studentName) || string.IsNullOrEmpty(studentNo)
                    || string.IsNullOrEmpty(studentPassword) || string.IsNullOrEmpty(studentEmail))
                {
                    ViewBag.Message = "All student fields are required!";
                    return View();
                }

                if (_context.Students.Any(s => s.StudentNo == studentNo))
                {
                    ViewBag.Message = "Student number already exists!";
                    return View();
                }

                var student = new Student
                {
                    StudentName = studentName,
                    StudentNo = studentNo,
                    Email = studentEmail,
                    Password = BCrypt.Net.BCrypt.HashPassword(studentPassword),
                    IsApproved = false
                };
                _context.Students.Add(student);
                _context.SaveChanges();

                ViewBag.Message = "Student registered successfully!";
                return RedirectToAction("RegistrationPending", "Registration");
            }

            ViewBag.Message = "Invalid role selection!";
            return View();
        }
        public IActionResult RegistrationPending()
        {
            return View(); 
        }
    }

}
