using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AttendanceSystem.Models
{
    public class Attendance
    {
        public int Id { get; set; }

        [Required]
        public int StudentId { get; set; }

        [Required]
        public int SubjectId { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public TimeSpan TimeIn { get; set; }
        public TimeSpan? TimeOut { get; set; }
        public TimeSpan? Duration => TimeOut.HasValue ? TimeOut.Value - TimeIn : null;

        [ForeignKey("StudentId")]
        public Student? Student { get; set; }

        [ForeignKey("SubjectId")]
        public Subject? Subject { get; set; }
    }
}
