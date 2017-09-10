using System;

namespace CoursesApi.Repositories.Exceptions
{
	public class AlreadyInCourseException: Exception
	{
		public AlreadyInCourseException()
			: base("The student with the given Id is already in the course with the given Id.")
		{ }
	}
}