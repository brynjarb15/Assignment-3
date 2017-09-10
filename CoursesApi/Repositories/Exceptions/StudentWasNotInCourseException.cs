using System;

namespace CoursesApi.Repositories.Exceptions
{
	public class StudentWasNotInCourseException: Exception
	{
		public StudentWasNotInCourseException()
			: base("The student with the given Id was not enrolled in the course with the given Id.")
		{ }
	}
}