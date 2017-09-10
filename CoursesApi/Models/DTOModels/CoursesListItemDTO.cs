using System;

namespace CoursesApi.Models.DTOModels
{
	/// <summary>
	/// A minimal version of course which is exposed to the user.
	/// </summary>
	public class CoursesListItemDTO
	{
		/// <summary>
		/// The id of the course
		/// </summary>
		/// <returns></returns>
		public int Id { get; set; }
		/// <summary>
		/// The name of the course
		/// </summary>
		/// <returns></returns>
		public string Name { get; set; }
		/// <summary>
		/// Number of students currently enrolled in the course
		/// </summary>
		/// <returns></returns>
		public int NumberOfStudents { get; set; }
	}
}