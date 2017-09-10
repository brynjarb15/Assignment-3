using CoursesApi.Models.EntityModels;
using Microsoft.EntityFrameworkCore;

namespace CoursesApi.Repositories
{
	public class AppDataContext : DbContext
	{
		public AppDataContext(DbContextOptions<AppDataContext> options)
			: base(options)
		{ }
		public DbSet<Course> Courses { get; set; }
		public DbSet<Student> Students { get; set; }
		public DbSet<CourseTemplate> CourseTemplates { get; set; }
		public DbSet<Enrollment> Enrollments { get; set; }
		public DbSet<WaitingList> WaitingList { get; set; }
	}
}	