using Microsoft.AspNetCore.Mvc;
using AttendanceSystem.Data;
using BCrypt.Net;
using System.Linq;

namespace AttendanceSystem.Controllers
{
    public class LoginController : Controller
    {
        private readonly AppDbContext _context;

        public LoginController(AppDbContext context)
        {
            _context = context;
        }

        // Show login page
        public IActionResult Index() => View();

        [HttpPost]
        public IActionResult Index(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                ViewBag.Error = "Please fill in all fields!";
                return View();
            }

            // Check if Admin (Teacher)
            var admin = _context.Admins.FirstOrDefault(a => a.Username == username);
            if (admin != null && BCrypt.Net.BCrypt.Verify(password, admin.Password))
            {
                if (!admin.IsApproved)
                {
                    ViewBag.Error = "Your account is pending admin approval.";
                    return View();
                }
                HttpContext.Session.SetString("Role", "Admin");
                HttpContext.Session.SetString("Username", admin.Username);
                return RedirectToAction("Dashboard", "Home");
            }

            // Check if Student (by StudentNo or Email)
            var student = _context.Students.FirstOrDefault(s => s.StudentNo == username || s.Email == username);
            if (student != null && BCrypt.Net.BCrypt.Verify(password, student.Password))
            {
                if (!student.IsApproved)
                {
                    ViewBag.Error = "Your account is pending admin approval.";
                    return View();
                }
                HttpContext.Session.SetString("Role", "Student");
                HttpContext.Session.SetString("StudentName", student.StudentName);
                return RedirectToAction("StudentDashboard", "Home");
            }

            ViewBag.Error = "Invalid username/email or password!";
            return View();
        }

        // Logout
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }
    }
}
