using System.ComponentModel.DataAnnotations;

namespace AttendanceSystem.Models
{
    public class Student
    {
        public int Id { get; set; }

        [Required]
        public string StudentName { get; set; } = "";

        [Required]
        public string StudentNo { get; set; } = "";

        [Required, EmailAddress]
        public string Email { get; set; } = "";

        [Required]
        public string Password { get; set; } = "";

        public bool IsApproved { get; set; } = false;

    }
}
