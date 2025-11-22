using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AttendanceSystem.Data;
using AttendanceSystem.Models;

namespace AttendanceSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        // Admin Dashboard
        public IActionResult Dashboard()
        {
            var role = HttpContext.Session.GetString("Role");
            if (role != "Admin") return RedirectToAction("Index", "Login");

            var attendanceList = _context.Attendances
                .Include(a => a.Student)
                .Include(a => a.Subject)
                .OrderByDescending(a => a.Date)
                .ToList();

            ViewBag.Username = HttpContext.Session.GetString("Username");
            return View(attendanceList);
        }
        public IActionResult PendingUsers()
        {
            if (HttpContext.Session.GetString("Role") != "Admin") return RedirectToAction("Index", "Login");

            var pendingAdmins = _context.Admins.Where(a => !a.IsApproved).ToList();
            var pendingStudents = _context.Students.Where(s => !s.IsApproved).ToList();

            ViewBag.PendingAdmins = pendingAdmins;
            ViewBag.PendingStudents = pendingStudents;
            ViewBag.Username = HttpContext.Session.GetString("Username");
            return View();
        }
        // Student Dashboard
        public IActionResult StudentDashboard()
        {
            var role = HttpContext.Session.GetString("Role");
            if (role != "Student") return RedirectToAction("Index", "Login");

            var studentName = HttpContext.Session.GetString("StudentName");
            var attendanceList = _context.Attendances
                .Include(a => a.Subject)
                .Include(a => a.Student)
                .Where(a => a.Student.StudentName == studentName)
                .OrderByDescending(a => a.Date)
                .ToList();

            ViewBag.StudentName = studentName;
            return View(attendanceList);
        }
        [HttpPost]
        public IActionResult ApproveAdmin(int id)
        {
            if (HttpContext.Session.GetString("Role") != "Admin") return RedirectToAction("Index", "Login");

            var student = _context.Admins.Find(id);
            if (student != null)
            {
                student.IsApproved = true;
                _context.SaveChanges();
                TempData["Success"] = "Admin approved successfully!";
            }
            return RedirectToAction("PendingUsers");
        }
        [HttpPost]
        public IActionResult ApproveStudent(int id)
        {
            if (HttpContext.Session.GetString("Role") != "Admin") return RedirectToAction("Index", "Login");

            var student = _context.Students.Find(id);
            if (student != null)
            {
                student.IsApproved = true;
                _context.SaveChanges();
                TempData["Success"] = "Student approved successfully!";
            }
            return RedirectToAction("PendingUsers");
        }
        // Reject Admin (Delete pending admin)
        [HttpPost]
        public IActionResult RejectAdmin(int id)
        {
            if (HttpContext.Session.GetString("Role") != "Admin") return RedirectToAction("Index", "Login");

            var admin = _context.Admins.FirstOrDefault(a => a.Id == id && !a.IsApproved);
            if (admin != null)
            {
                _context.Admins.Remove(admin);
                _context.SaveChanges();
                TempData["Success"] = "Admin registration rejected and removed.";
            }
            return RedirectToAction("PendingUsers");
        }

        // Reject Student (Delete pending student)
        [HttpPost]
        public IActionResult RejectStudent(int id)
        {
            if (HttpContext.Session.GetString("Role") != "Admin") return RedirectToAction("Index", "Login");

            var student = _context.Students.FirstOrDefault(s => s.Id == id && !s.IsApproved);
            if (student != null)
            {
                _context.Students.Remove(student);
                _context.SaveChanges();
                TempData["Success"] = "Student registration rejected and removed.";
            }
            return RedirectToAction("PendingUsers");
        }
        // Add Attendance (GET)
        [HttpGet]
        public IActionResult AddAttendance()
        {
            if (HttpContext.Session.GetString("Role") != "Admin") return RedirectToAction("Index", "Login");

            ViewBag.Students = _context.Students.ToList();
            ViewBag.Subjects = _context.Subjects.ToList();
            return View();
        }

        // Add Attendance (POST)
        [HttpPost]
        public IActionResult AddAttendance(Attendance model)
        {
            if (HttpContext.Session.GetString("Role") != "Admin") return RedirectToAction("Index", "Login");

            if (ModelState.IsValid)
            {
                model.Date = DateTime.Now.Date;
                model.TimeIn = DateTime.Now.TimeOfDay;

                _context.Attendances.Add(model);
                _context.SaveChanges();

                TempData["Success"] = "Attendance successfully recorded!";
                return RedirectToAction("Dashboard");
            }

            ViewBag.Students = _context.Students.ToList();
            ViewBag.Subjects = _context.Subjects.ToList();
            return View(model);
        }

        // Edit Attendance (GET)
        [HttpGet]
        public IActionResult EditAttendance(int id)
        {
            if (HttpContext.Session.GetString("Role") != "Admin") return RedirectToAction("Index", "Login");

            var attendance = _context.Attendances.Find(id);
            if (attendance == null) return NotFound();

            ViewBag.Students = _context.Students.ToList();
            ViewBag.Subjects = _context.Subjects.ToList();

            return View(attendance);
        }

        // Edit Attendance (POST) - Updated to include Time Out
        [HttpPost]
        public IActionResult EditAttendance(Attendance model)
        {
            if (HttpContext.Session.GetString("Role") != "Admin") return RedirectToAction("Index", "Login");

            if (ModelState.IsValid)
            {
                // Validation: Ensure Time Out is after Time In
                if (model.TimeOut.HasValue && model.TimeOut.Value <= model.TimeIn)
                {
                    ModelState.AddModelError("TimeOut", "Time Out must be after Time In.");
                    ViewBag.Students = _context.Students.ToList();
                    ViewBag.Subjects = _context.Subjects.ToList();
                    return View(model);
                }

                var attendance = _context.Attendances.Find(model.Id);
                if (attendance == null) return NotFound();

                attendance.StudentId = model.StudentId;
                attendance.SubjectId = model.SubjectId;
                attendance.Date = model.Date;
                attendance.TimeIn = model.TimeIn;
                attendance.TimeOut = model.TimeOut;  // New

                _context.SaveChanges();

                TempData["Success"] = "Attendance updated successfully!";
                return RedirectToAction("Dashboard");
            }

            ViewBag.Students = _context.Students.ToList();
            ViewBag.Subjects = _context.Subjects.ToList();
            return View(model);
        }

        // Delete Attendance
        [HttpPost]
        public IActionResult DeleteAttendance(int id)
        {
            if (HttpContext.Session.GetString("Role") != "Admin") return RedirectToAction("Index", "Login");

            var attendance = _context.Attendances.Find(id);
            if (attendance == null) return NotFound();

            _context.Attendances.Remove(attendance);
            _context.SaveChanges();

            TempData["Success"] = "Attendance deleted successfully!";
            return RedirectToAction("Dashboard");
        }

        // Check Out (Student self-service) - New
        [HttpPost]
        public IActionResult CheckOut(int attendanceId)
        {
            var role = HttpContext.Session.GetString("Role");
            if (role != "Student") return RedirectToAction("Index", "Login");

            var studentName = HttpContext.Session.GetString("StudentName");
            var attendance = _context.Attendances
                .Include(a => a.Student)
                .FirstOrDefault(a => a.Id == attendanceId && a.Student.StudentName == studentName);

            if (attendance == null || attendance.TimeOut.HasValue)
                return NotFound();  // Already checked out or not found

            attendance.TimeOut = DateTime.Now.TimeOfDay;
            _context.SaveChanges();

            TempData["Success"] = "Checked out successfully!";
            return RedirectToAction("StudentDashboard");
        }

        // Logout
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Login");
        }
    }
}
