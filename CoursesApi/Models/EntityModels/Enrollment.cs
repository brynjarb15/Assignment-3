namespace CoursesApi.Models.EntityModels
{
	/// <summary>
	/// This is a entity which works as a link between a student and course.
	/// </summary>
	public class Enrollment
	{
		/// <summary>
		/// The id of the enrollment
		/// </summary>
		/// <returns></returns>
		public int Id { get; set; }
		/// <summary>
		/// The course id
		/// </summary>
		/// <returns></returns>
		public int CourseId { get; set; }
		/// <summary>
		/// The social security number (kennitala) if the student
		/// </summary>
		/// <returns></returns>
		public string StudentSSN { get; set; }
		/// <summary>
		/// Is true if student has been removed from course
		/// </summary>
		/// <returns></returns>
		public bool NotRemoved { get; set;}
	}
}