using System.ComponentModel.DataAnnotations;

namespace AttendanceSystem.Models
{
    public class Admin
    {
        public int Id { get; set; }

        [Required]
        public string Username { get; set; } = "";

        [Required]
        public string Password { get; set; } = "";
        public bool IsApproved { get; set; } = false;

    }
}
