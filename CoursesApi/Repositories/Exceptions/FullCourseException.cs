using System;

namespace CoursesApi.Repositories.Exceptions
{
	public class FullCourseException: Exception
	{
		public FullCourseException()
			: base("The course with the given id is full so no more students can join it")
		{ }
	}
}