using System;

namespace CoursesApi.Repositories.Exceptions
{
	public class StudentNotFoundException: Exception
	{
		public StudentNotFoundException()
			: base("The student with the given Id could not be found.")
		{ }
	}
}