using Microsoft.EntityFrameworkCore;
using AttendanceSystem.Models;

namespace AttendanceSystem.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Admin> Admins { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Attendance> Attendances { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seed Subjects
            modelBuilder.Entity<Subject>().HasData(
                new Subject { Id = 1, SubjectName = "IT315 (Web Information System)" },
                new Subject { Id = 2, SubjectName = "IT316 (ICT Trends)" },
                new Subject { Id = 3, SubjectName = "IT317 (C#.Net Programming)" },
                new Subject { Id = 4, SubjectName = "IT318 (VB.Net Programming)" },
                new Subject { Id = 5, SubjectName = "IT319 (Project Management for IT)" },
                new Subject { Id = 6, SubjectName = "IT Elect 1 (IT Elective 1)" },
                new Subject { Id = 7, SubjectName = "Phys 1 (Advanced Physics)" }
            );

            // Seed Default Admin
            // Username: admin
            // Password: admin123
            modelBuilder.Entity<Admin>().HasData(
                new Admin
                {
                    Id = 1,
                    Username = "admin",
                    Password = BCrypt.Net.BCrypt.HashPassword("admin123"),
                    IsApproved = true
                }
            );
        }
    }
}