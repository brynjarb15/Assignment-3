using System;

namespace CoursesApi.Repositories.Exceptions
{
	public class AlreadyOnWaitingListException : Exception
	{
		public AlreadyOnWaitingListException()
			: base("This student is already on the waiting list for the course with the given Id.")
		{ }
	}
}