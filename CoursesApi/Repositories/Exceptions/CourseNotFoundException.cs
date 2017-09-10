using System;

namespace CoursesApi.Repositories.Exceptions
{
	public class CourseNotFoundException: Exception
	{
		public CourseNotFoundException()
			: base("A course with the given Id could not be found.")
		{ }
	}
}