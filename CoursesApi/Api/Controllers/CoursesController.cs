using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoursesApi.Models.DTOModels;
using CoursesApi.Models.EntityModels;
using CoursesApi.Models.ViewModels;
using CoursesApi.Repositories.Exceptions;
using CoursesApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
	/// <summary>
	/// A resource for courses
	/// </summary>
	[Route("api/[controller]")]
	public class CoursesController : Controller
	{
		private ICoursesService _coursesService;

		public CoursesController(ICoursesService coursesService)
		{
			  _coursesService = coursesService;
		}

		/// <summary>
		/// A route which should receive all courses from the database
		/// </summary>
		/// <param name="semester">The semester which filters the courses</param>
		/// <returns>A list of CourseDTO's</returns>
		[HttpGet]
		public IActionResult GetCourses(string semester = "20173")
		{
			var courses = _coursesService.GetCourses(semester);
			
			return Ok(courses);
		}

		/// <summary>
		/// A route which should return a course by providing a valid id
		/// </summary>
		/// <param name="courseId">An integer id for a course</param>
		/// <returns>A single CourseDTO object</returns>
		[HttpGet]
		[Route("{courseId:int}", Name = "GetCourseById")]
		public IActionResult GetCourseById(int courseId)
		{
			try
			{
				var courses = _coursesService.GetCourseById(courseId);
				return Ok(courses);
			}
			catch(CourseNotFoundException e)
			{
				return NotFound(e.Message);
			}

			
		}

		[HttpPost]
		[Route("", Name = "AddCourse")]
		public IActionResult AddCourse([FromBody] CourseViewModel course) 
		{
			if (course == null) { return BadRequest(); }
			if (!ModelState.IsValid) { return StatusCode(412); }

			var newCourse = _coursesService.AddCourse(course);

			return Ok(newCourse);
		}

		/// <summary>
		/// Updates a course
		/// </summary>
		/// <param name="courseId">An integer id for a course</param>
		/// <param name="updatedCourse">The updated values for the course</param>
		/// <returns>The updated course</returns>
		[HttpPut]
		[Route("{courseId:int}")]
		public IActionResult UpdateCourse(int courseId, [FromBody] CourseViewModel updatedCourse)
		{
			if (updatedCourse == null) { return BadRequest(); }
			if (!ModelState.IsValid) { return StatusCode(412); }

			try 
			{
				var course = _coursesService.UpdateCourse(courseId, updatedCourse);
				return Ok(course);
			}
			catch(CourseNotFoundException e)
			{
				return NotFound(e.Message);
			}

			
		}

		/// <summary>
		/// Retrieves a list of students which are enrolled in a given course
		/// </summary>
		/// <param name="courseId">The id of the course</param>
		/// <returns>A list of StudentDTO's</returns>
		[HttpGet]
		[Route("{courseId:int}/students")]
		public IActionResult GetStudentsByCourseId(int courseId)
		{
			try
			{
				var students = _coursesService.GetStudentsByCourseId(courseId);
				return Ok(students);
			}
			catch(CourseNotFoundException e)
			{
				return NotFound(e.Message);
			}


			
		}

		/// <summary>
		/// Adds a student to a course. Student must be already in the system
		/// </summary>
		/// <param name="courseId">The id of the course</param>
		/// <param name="newStudent">The new student to add</param>
		/// <returns>The newly created student</returns>
		[HttpPost]
		[Route("{courseId:int}/students")]
		public IActionResult AddStudentToCourse(int courseId, [FromBody] StudentViewModel newStudent)
		{
			if (newStudent == null) { return BadRequest(); }
			if (!ModelState.IsValid) { return StatusCode(412); }

			try 
			{
				var response = _coursesService.AddStudentToCourse(courseId, newStudent);
				return Ok(response);
			}
			catch(StudentNotFoundException e)
			{
				return NotFound(e.Message);
			}
			catch(CourseNotFoundException e)
			{
				return NotFound(e.Message);
			}
			catch(FullCourseException e)
			{
				return StatusCode(412, e.Message);
			}
			catch(AlreadyInCourseException e)
			{
				return StatusCode(412, e.Message);
			}
		}

		/// <summary>
		/// Deletes a student from a course
		/// </summary>
		/// <param name="courseId">The id of the course</param>
		/// <returns>A status code 204 (if successful)</returns>
		[HttpDelete]
		[Route("{courseId:int}")]
		public IActionResult DeleteCourse(int courseId)
		{
			try {
				var success = _coursesService.DeleteCourseById(courseId);
				return NoContent();
			}
			catch(CourseNotFoundException e)
			{
				return NotFound(e.Message);
			}
			
		}

		/// <summary>
		/// returns a list of all students on the waiting list for a course
		/// </summary>
		/// <param name="courseId">id of the course</param>
		/// <returns>list of students</returns>
		[HttpGet]
		[Route("{courseId}/waitinglist")]
		public IActionResult GetWaitinglistForCourse(int courseId)
		{
			try {
				var students = _coursesService.GetWaitinglistForCourse(courseId);
				return Ok(students);
			}
			catch(CourseNotFoundException e)
			{
				return NotFound(e.Message);
			}
		}
		
		 
		/// <summary>
		/// Adds a student to the waiting list for a course
		/// </summary>
		/// <param name="student">The student to add</param>
		/// <param name="courseId">id of the course</param>
		/// <returns></returns>
		[HttpPost]
		[Route("{courseId}/waitinglist")]
		public IActionResult AddToWaitinglist([FromBody] StudentViewModel student, int courseId)
		{
			if (student == null) { return BadRequest(); }
			if (!ModelState.IsValid) { return StatusCode(412); }
			try
			{
				var waitingList = _coursesService.AddToWaitinglist(student, courseId);
				return Ok(waitingList);
			}
			catch(StudentNotFoundException e)
			{
				return NotFound(e.Message);
			}
			catch(CourseNotFoundException e)
			{
				return NotFound(e.Message);
			}
			catch(AlreadyOnWaitingListException e)
			{
				return StatusCode(412, e.Message);
			}
			catch(AlreadyInCourseException e)
			{
				return StatusCode(412, e.Message); 
			}
		}

		/// <summary>
		/// Removes a student enrollment in a course
		/// </summary>
		/// <param name="courseId">id of the course the student is enrolled in</param>
		/// <param name="ssn">ssn of the student to be removed</param>
		/// <returns>A status code 204 (if successful)</returns>
		[HttpDelete]
		[Route("{courseId:int}/students/{ssn}")]
		public IActionResult RemoveStudentFromCourse(int courseId, string ssn)
		{
			try
			{
				_coursesService.RemoveStudentFromCourse(courseId, ssn);
				return NoContent();
			}
			catch(StudentNotFoundException e)
			{
				return NotFound(e.Message);
			}
			catch(CourseNotFoundException e)
			{
				return NotFound(e.Message);
			}
			catch(StudentWasNotInCourseException e)
			{
				return StatusCode(412, e.Message);
			}
		}
	}
}
