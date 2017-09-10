using System;

namespace CoursesApi.Models.EntityModels
{
	/// <summary>
	/// Entity model for the course. This is a direct mapping from the database and should not be exposed to public users.
	/// </summary>
	public class Course
	{
		/// <summary>
		/// Id of the course
		/// </summary>
		/// <returns></returns>
		public int Id { get; set; }
		/// <summary>
		/// A course template which this course is structured from
		/// </summary>
		/// <returns></returns>
		public string CourseTemplate { get; set; }
		/// <summary>
		/// The starting date of the course
		/// </summary>
		/// <returns></returns>
		public DateTime? StartDate { get; set; }
		/// <summary>
		/// The end date of the course
		/// </summary>
		/// <returns></returns>
		public DateTime? EndDate { get; set; }
		/// <summary>
		/// The semester this course is on
		/// </summary>
		/// <returns></returns>
		public string Semester { get; set; }

		/// <summary>
		/// The maximum number of students that can be in the course
		/// </summary>
		/// <returns></returns>
		public int MaxStudents { get; set; }
	}
}