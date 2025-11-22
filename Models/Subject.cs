using System.ComponentModel.DataAnnotations;

namespace AttendanceSystem.Models
{
    public class Subject
    {
        public int Id { get; set; }

        [Required]
        public string SubjectName { get; set; } = "";
    }
}
